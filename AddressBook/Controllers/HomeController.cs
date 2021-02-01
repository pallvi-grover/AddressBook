using System;
using System.Collections.Generic;
using System.IO;
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
        private ApplicationDbContext db = new ApplicationDbContext();
        private ReferenceVariable hardCodedObjects = new ReferenceVariable();

        public async Task<ActionResult> Index(string email)
        {
            if (Request.IsAuthenticated || email != null)
            {
                ViewBag.Authenticate = true;
                List<ContactsInfo> test = new List<ContactsInfo>();
                var userId = db.Users.Where(i => i.Email == email).Select(i => i.Id).FirstOrDefault();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(hardCodedObjects.localhostURL);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync(hardCodedObjects.getAPIURL + userId);
                    if (Res.IsSuccessStatusCode)
                    {
                        TempData["UserID"] = userId;
                        var ampresp = Res.Content.ReadAsStringAsync().Result;
                        test = JsonConvert.DeserializeObject<List<ContactsInfo>>(ampresp);
                        TempData.Keep("UserID");
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
            ViewBag.Authenticate = true;
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


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Authenticate = true;
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

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpContext.User =
    new GenericPrincipal(new GenericIdentity(string.Empty), null);
            if (!Request.IsAuthenticated)
            {
                ViewBag.Authenticate = Request.IsAuthenticated;
                return RedirectToAction("Login", "Home");
            }
            else
                return View();
        }

        public string getEmailId(string userId)
        {
            var emailId = db.Users.Where(i => i.Id == userId).Select(i => i.Email).FirstOrDefault();
            return emailId;
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;
                        string fileId = Guid.NewGuid().ToString().Replace("-", "");
                        var ext = Path.GetExtension(file.FileName);
                        if (hardCodedObjects.allowedExtensions.Contains(ext))
                        {
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }
                            string myfile = fname + "_" + fileId + ext;
                            // Get the complete folder path and store the file inside it.  
                            fname = Path.Combine(Server.MapPath(hardCodedObjects.imagePathURL), myfile);
                            file.SaveAs(fname);

                        }
                        else
                            return Json("Cannot upload this type of file!");
                    }
                    return Json("File Uploaded Successfully!");

                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

    }
}
