using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AddressBook.Models;

namespace AddressBook.Controllers
{
    public class HomeController : Controller
    {
        private contactsDBContext db = new contactsDBContext();
        public ActionResult Index()
        {
            //var employees = from e in db.ContactsInfos
            //                orderby e.ID
            //                select e;
            
            ViewBag.Title = "Home Page";

            // return View(employees);
            return View();
        }
    }
}
