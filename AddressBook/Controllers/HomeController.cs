﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AddressBook.Models;
using Newtonsoft.Json;

namespace AddressBook.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}
        //private contactsDBContext db = new contactsDBContext();
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index(string email)
        {
            if (Request.IsAuthenticated || email != null)
            {
                List<ContactsInfo> test = new List<ContactsInfo>();
                var userId = db.Users.Where(i => i.Email == email).Select(i => i.Id).FirstOrDefault();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44336");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("api/Contacts/0?userId=" + userId);
                    if (Res.IsSuccessStatusCode)
                    {
                        TempData["UserID"] = userId;
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
            else
            {
                return View("Login");
            }
        }

        public ActionResult Edit(int id)
        {
            string email = string.Empty;
            if (TempData.ContainsKey("UserID"))
                email = TempData["UserID"] as string;
            if (email != null)
            {
                TempData.Keep("UserID");
                var employee = (db.ContactsInfos.Where(y => y.ID == id).Select(x => new { Contacts = x, x.phoneNumbers })).ToList();
                return View(employee[0].Contacts);
            }
            else
                return View("Login");

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

        [AllowAnonymous]
        public ActionResult Login()
        {
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return View();
        }

        public ActionResult Create()
        {
            string email = string.Empty;
            if (TempData.ContainsKey("UserID"))
                email = TempData["UserID"] as string;
            if (email != null)
            {
                TempData.Keep("UserID");
                return View(new ContactsInfo());
            }
            else
                return View("Login");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpContext.User =
    new GenericPrincipal(new GenericIdentity(string.Empty), null);
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fullName,nickName,emailID,dob,address")] ContactsInfo contacts, HttpPostedFileBase file)
        {
            var allowedExtensions = new[] {
            ".Jpg", ".png", ".jpg", "jpeg", ".JPG"
        };
            if (file != null)
            {
                //Extract Image File Name.
                string fileName = System.IO.Path.GetFileName(file.FileName);
                //string fileId = Guid.NewGuid().ToString().Replace("-", "");
                var ext = System.IO.Path.GetExtension(file.FileName);
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + ext; //appending the name with id  
                                                // store the file inside ~/project folder(Img)  
                    var filePath = System.IO.Path.Combine(Server.MapPath("~/ContactImages"), myfile);
                    //Save the Image File in Folder.
                    file.SaveAs(filePath);

                    //Insert the Image File details in Table.

                    //db.ContactsInfos.Add(new ContactsInfo
                    //{
                    //    imageURL = filePath
                    //});
                    //db.SaveChanges();
                }
                //Redirect to Index Action.
                
            }
            //return RedirectToAction("Index");
            return View();
        }
        public string getEmailId(string userId)
        {
            var emailId = db.Users.Where(i => i.Id == userId).Select(i => i.Email).FirstOrDefault();
            return emailId;
        }

    }
}
