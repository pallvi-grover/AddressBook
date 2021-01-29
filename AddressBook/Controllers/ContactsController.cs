using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using AddressBook.Models;
using NLog;

namespace AddressBook.Controllers
{
    public class ContactsController : ApiController
    {
        //private contactsDBContext db = new contactsDBContext();
        private ApplicationDbContext db = new ApplicationDbContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/Contacts
        public IQueryable<ContactsInfo> GetContactsInfos()
        {
            return db.ContactsInfos.OrderBy(i => i.fullName);
        }

        // GET: api/Contacts/5
        [ResponseType(typeof(ContactsInfo))]
        public IHttpActionResult GetContactsInfo(int id)
        {
            ContactsInfo contactsInfo = db.ContactsInfos.Find(id);
            if (contactsInfo == null)
            {
                logger.Error(NotFound());
                return NotFound();
            }
            logger.Info("Fetched the records for Contact ID: " + id);
            return Ok(contactsInfo);
        }

        public IQueryable<ContactsInfo> GetContactsInfosBasedOnUser(int id, string userId)
        {
            logger.Info("This is the GET Request to API. Fetched the records for User ID: " + userId);
            return db.ContactsInfos.Where(i => i.applicationUserId == userId).OrderBy(i => i.fullName);
        }

        // PUT: api/Contacts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContactsInfo(int id, ContactsInfo contactsInfo)
        {
            logger.Info("This is the PUT Request to API to update the contact information");
            if (!ModelState.IsValid)
            {
                logger.Error(BadRequest(ModelState));
                return BadRequest(ModelState);
            }

            if (id != contactsInfo.ID)
            {
                logger.Error(BadRequest());
                return BadRequest();
            }

            db.Entry(contactsInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                logger.Info("Database changes committed successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsInfoExists(id))
                {
                    logger.Error(NotFound());
                    return NotFound();
                }
                else
                {
                    logger.Fatal("Error occured while committing DB changes");
                    throw;
                }
            }
            logger.Info("PUT request for contact executed successfully");
            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutPhoneNumber(int id, int pid, PhoneNumbers phoneNumbers)
        {
            logger.Info("This is the PUT Request to API to update Phone Number");
            if (!ModelState.IsValid)
            {
                logger.Error(BadRequest(ModelState));
                return BadRequest(ModelState);
            }

            if (pid != phoneNumbers.ID)
            {
                logger.Error(BadRequest());
                return BadRequest();
            }

            db.Entry(phoneNumbers).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                logger.Info("Database changes committed successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsInfoExists(pid))
                {
                    logger.Error(NotFound());
                    return NotFound();
                }
                else
                {
                    logger.Fatal("Error occured while committing DB changes");
                    throw;
                }
            }

            catch (Exception e) {
                var ex = e.InnerException.InnerException.Message;

                if (ex.Contains("Cannot insert duplicate key row"))
                {
                    return BadRequest("Phone Number already exists. Please enter unique number.");
                }
                else
                {
                    return BadRequest(ex.ToString());
                }

            }

            logger.Info("PUT request for phone numbers executed successfully");

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Contacts
        [ResponseType(typeof(ContactsInfo))]
        public IHttpActionResult PostContactsInfo(ContactsInfo contactsInfo)
        {
            logger.Info("This is the POST Request to API to update Contact");
            if (!ModelState.IsValid)
            {
                logger.Error(BadRequest(ModelState));
                return BadRequest(ModelState);
            }

            db.ContactsInfos.Add(contactsInfo);
            try
            {
                db.SaveChanges();
                logger.Info("Database changes committed successfully");
            }
            catch (Exception e)
            {
                var ex = e.InnerException.InnerException.Message;

                if (ex.Contains("The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value"))
                {
                    logger.Fatal(BadRequest("DOB Value Out of Range."));
                    return BadRequest("DOB Value Out of Range.");
                }
                else
                {
                    logger.Fatal(BadRequest(ex.ToString()));
                    return BadRequest(ex.ToString());
                }
                //The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value


            }
            logger.Info("POST request for contacts executed successfully");
            return Ok();
        }

        [ResponseType(typeof(PhoneNumbers))]
        public IHttpActionResult PostPhoneNumber(int id, PhoneNumbers phoneno)
        {
            logger.Info("This is the POST Request to API to update Phone Number");
            if (!ModelState.IsValid)
            {
                logger.Error(BadRequest(ModelState));
                return BadRequest(ModelState);
            }

            db.numbers.Add(phoneno);

            try
            {
                db.SaveChanges();
                logger.Info("Database changes committed successfully");
            }
            catch(Exception e) {
                var ex = e.InnerException.InnerException.Message;

                if (ex.Contains("Cannot insert duplicate key row"))
                {
                    return BadRequest("Phone Number already exists. Please enter unique number.");
                }
                else
                {
                    return BadRequest(ex.ToString());
                }
                //The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value

            }
            //return CreatedAtRoute("DefaultApi", new { id = contactsInfo.ID }, contactsInfo);

            
            logger.Info("POST request for phone numbers executed successfully");

            return Ok();
        }

        // DELETE: api/Contacts/5
        [ResponseType(typeof(ContactsInfo))]
        public IHttpActionResult DeleteContactsInfo(int id)
        {
            logger.Info("This is the DELETE Request to API to delete contact");
            ContactsInfo contactsInfo = db.ContactsInfos.Find(id);
            if (contactsInfo == null)
            {
                logger.Error(NotFound());
                return NotFound();
            }

            db.ContactsInfos.Remove(contactsInfo);
            db.SaveChanges();
            logger.Info("Database changes committed successfully");
            logger.Info("DELETE request for contacts executed successfully for " + Ok(contactsInfo));
            return Ok(contactsInfo);
        }

        [ResponseType(typeof(PhoneNumbers))]
        public IHttpActionResult DeletePhoneNumber(int id, int pid)
        {
            logger.Info("This is the DELETE Request to API to delete phone number");
            PhoneNumbers contactsInfo = db.numbers.Find(pid);
            if (contactsInfo == null)
            {
                logger.Error(NotFound());
                return NotFound();
            }

            db.numbers.Remove(contactsInfo);
            db.SaveChanges();
            logger.Info("Database changes committed successfully");
            logger.Info("DELETE request for phone number executed successfully for " + Ok(contactsInfo));
            return Ok(contactsInfo);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactsInfoExists(int id)
        {
            return db.ContactsInfos.Count(e => e.ID == id) > 0;
        }
    }
}