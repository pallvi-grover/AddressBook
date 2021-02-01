using AddressBook.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AddressBook.Controllers;
using System.Net;
using System.Threading;
using System.Linq;

namespace AddressBook.Tests.Controllers

{
    [TestClass]
    public class ContactsControllerTest : ApiController
    {
        [TestMethod]
        public void getContacts()
        {
            var controller = new ContactsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            IQueryable<ContactsInfo> response = controller.GetContactsInfosBasedOnUser(1, null);

            Assert.IsNull(response);
            //Assert.IsTrue(response.All(n => string.Equals(n.nickName, "test")));
            //Assert.IsTrue(response.All(n => string.Equals(n.emailID, "test02@test.com")));

        }

        [TestMethod]
        public void UpdateContact()
        {
            var controller = new ContactsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            //IQueryable<ContactsInfo> response = controller.GetContactsInfosBasedOnUser(1, null);
            //var id = response.Select(i => i.ID).ToList();
            ContactsInfo Update = new ContactsInfo
            {
                fullName = "test01",
                nickName = "test",
                emailID = "test02@test.com",
                dob = Convert.ToDateTime("09/03/1989"),
                address = "Canada",
                //ID = id[0]
            };

            var update = controller.PutContactsInfo(1, Update);
            Assert.AreEqual(HttpStatusCode.BadRequest, update.ExecuteAsync(CancellationToken.None).Result.StatusCode);

        }

        [TestMethod]
        public void AddContact()
        {
            var controller = new ContactsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var response = controller.PostContactsInfo(GetContact()[1]);
            Assert.AreEqual(HttpStatusCode.OK, response.ExecuteAsync(CancellationToken.None).Result.StatusCode);

        }

        private List<ContactsInfo> GetContact()
        {
            var testContacts = new List<ContactsInfo>();
            testContacts.Add(new ContactsInfo
            {
                fullName = "test01",
                nickName = "test",
                emailID = "test01@test.com",
                dob = Convert.ToDateTime("09/03/1989"),
                address = "Canada",

            });
            testContacts.Add(new ContactsInfo
            {
                fullName = "test02",
                nickName = "test",
                emailID = "test02@test.com",
                dob = Convert.ToDateTime("09/05/1988"),
                address = "USA"
            });
            testContacts.Add(new ContactsInfo
            {
                fullName = "test03",
                nickName = "test",
                emailID = "test02@test.com",
                dob = Convert.ToDateTime("09/03/1969"),
                address = "England"
            });
            testContacts.Add(new ContactsInfo
            {
                fullName = "test04",
                nickName = "test",
                emailID = "test04@test.com",
                dob = Convert.ToDateTime("09/03/1977"),
                address = "France"
            });

            return testContacts;
        }
    }
}
