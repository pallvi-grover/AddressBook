﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AddressBook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace AddressBook.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private contactsDBContext db = new contactsDBContext();
        public async Task<ActionResult> Index()
        {
            //var employees = from e in db.ContactsInfos
            //                orderby e.ID
            //                select e;

            //ViewBag.Title = "Home Page";

            // return View(employees);
            //return View();
            List<ContactsInfo> test = new List<ContactsInfo>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44336");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Contacts");
                if (Res.IsSuccessStatusCode)
                {
                    var ampresp = Res.Content.ReadAsStringAsync().Result;
                    test = JsonConvert.DeserializeObject<List<ContactsInfo>>(ampresp);
                    return View(test);

                }
                else
                {
                    return View("Login");
                }
            }
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

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Login(string button, FormCollection collection)
        //{
        //    // This will clear whatever form items have been populated        
        //    if (button == "Cancel")  //I can see code is executed here, I need to clear all UI fields starts from there.
        //    {
        //        //here, I need to clear all Input text fields, text, listbox, dropdownlist, etc.   
        //        ModelState.Clear();
        //        return View(); //this will return the original UI razor view,

        //    }
        //    return View();
        //}

        //[ChildActionOnly]
        //public ActionResult RenderMenu(int id)
        //{
        //    var employee = db.numbers.Where(m => m.contactId == id);
        //    return PartialView("_PhoneNumberRender",employee);
        //}

        //[HttpPost]
        //public void Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        var term = db.numbers.Where(x => x.ID == id).First();
        //        db.numbers.Remove(term);
        //        db.SaveChanges();
        //        //return View();
        //        //return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        //return View();
        //    }
        //}

        //[HttpDelete]
        //public JsonResult Delete(int id)
        //{
        //    //var term = db.numbers.Where(x => x.ID == id).First();
        //    //db.numbers.Remove(term);
        //    //db.SaveChanges();
        //    return Json("Response from Delete");
        //}

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Home");
        }
    }
}
