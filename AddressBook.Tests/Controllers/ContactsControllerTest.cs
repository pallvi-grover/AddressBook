using AddressBook.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using AddressBook.Controllers;
using System.Web.Http.Results;
using System.Net;
using System.Threading;

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

            var response = controller.GetContactsInfos();


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
                address = "Canada"
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
                emailID = "test03@test.com",
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
