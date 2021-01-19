using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class ContactsInfo
    {
        public int ID { get; set; }

        public string fullName { get; set; }

        public string nickName { get; set; }

        public string emailID { get; set; }

        public DateTime dob { get; set; }

        public string address { get; set; }

        public ICollection<PhoneNumbers> phoneNumbers { get; set; }
    }

    public class PhoneNumbers
    {
        public int ID { get; set; }

        public string type { get; set; }

        public string Number { get; set; }

        public int contactId { get; set; }
        public ContactsInfo contacts { get; set; }
    }
}