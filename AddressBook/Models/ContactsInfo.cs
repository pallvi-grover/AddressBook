using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class ContactsInfo
    {
        public int ID { get; set; }
        [StringLength(30, ErrorMessage = "Name should be only 30 characters long")]
        public string fullName { get; set; }

        [StringLength(15, ErrorMessage = "NickName should be only 15 characters long")]
        public string nickName { get; set; }

        [StringLength(320, ErrorMessage = "Too long email ID")]
        public string emailID { get; set; }
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }
        [StringLength(500)]
        public string address { get; set; }
        public string imageURL { get; set; }

        public string applicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }

        public ICollection<PhoneNumbers> phoneNumbers { get; set; }
    }

    public class PhoneNumbers
    {
        public int ID { get; set; }

        public string type { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(15, ErrorMessage = "Phone number can be maximum of 15 digits")]
        public string Number { get; set; }
    
        public int contactId { get; set; }
        public ContactsInfo contacts { get; set; }

    }
}