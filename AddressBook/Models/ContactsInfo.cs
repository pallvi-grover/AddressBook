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
        
        [Required(ErrorMessage ="Name cannot be null.")]
        public string fullName { get; set; }

        [StringLength(15, ErrorMessage = "NickName should be only 15 characters long")]
        public string nickName { get; set; }

        [Required(ErrorMessage = "Email ID cannot be null.")]
        [StringLength(320, ErrorMessage = "Too long email ID")]
        [RegularExpression(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", ErrorMessage = "Invalid email format. Please enter valid format")]        
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