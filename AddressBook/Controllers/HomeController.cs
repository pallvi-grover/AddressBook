using System;
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
            return View(new ContactsInfo());
        }

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
                return RedirectToAction("Index");
            }
            else
                return View();
        }


    }
}
