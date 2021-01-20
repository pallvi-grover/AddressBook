using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class contactsDBContext : DbContext
    {
        public DbSet<ContactsInfo> ContactsInfos { get; set; }
        public DbSet<PhoneNumbers> numbers { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
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