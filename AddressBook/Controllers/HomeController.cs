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
            var employees = from e in db.ContactsInfos
                            orderby e.ID
                            select e;

            ViewBag.Title = "Home Page";

             return View(employees);
            //return View();
        }

        public ActionResult Edit(int id)
        {
            var employee = (db.ContactsInfos.Where(y => y.ID == id).Select(x => new { Contacts = x, x.phoneNumbers })).ToList();
            return View(employee[0].Contacts);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var employee = db.ContactsInfos.Single(m => m.ID == id);
                if (TryUpdateModel(employee))
                {
                    //To Do:- database code
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(employee);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}
