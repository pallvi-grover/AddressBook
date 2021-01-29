using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AddressBook.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            //: base("DefaultConnection", throwIfV1Schema: false)
            : base("SQLConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }
        public DbSet<ContactsInfo> ContactsInfos { get; set; }
        public DbSet<PhoneNumbers> numbers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ContactsInfo>()
    .HasMany<PhoneNumbers>(g => g.phoneNumbers)
    .WithRequired(s => s.contacts)
    .HasForeignKey<int>(s => s.contactId)
    .WillCascadeOnDelete();

            modelBuilder.Entity<ContactsInfo>()
                .Property(e => e.emailID)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() { IsUnique = true }));
            modelBuilder.Entity<PhoneNumbers>()
                .Property(e => e.Number)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() { IsUnique = true }));
        }
    }
}