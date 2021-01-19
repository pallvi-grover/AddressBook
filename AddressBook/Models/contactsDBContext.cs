using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        }
    }
}