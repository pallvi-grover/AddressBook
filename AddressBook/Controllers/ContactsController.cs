using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AddressBook.Models;

namespace AddressBook.Controllers
{
    public class ContactsController : ApiController
    {
        private contactsDBContext db = new contactsDBContext();

        // GET: api/Contacts
        public IQueryable<ContactsInfo> GetContactsInfos()
        {
            return db.ContactsInfos;
        }

        // GET: api/Contacts/5
        [ResponseType(typeof(ContactsInfo))]
        public IHttpActionResult GetContactsInfo(int id)
        {
            ContactsInfo contactsInfo = db.ContactsInfos.Find(id);
            if (contactsInfo == null)
            {
                return NotFound();
            }

            return Ok(contactsInfo);
        }

        // PUT: api/Contacts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContactsInfo(int id, ContactsInfo contactsInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactsInfo.ID)
            {
                return BadRequest();
            }

            db.Entry(contactsInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Contacts
        [ResponseType(typeof(ContactsInfo))]
        public IHttpActionResult PostContactsInfo(ContactsInfo contactsInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ContactsInfos.Add(contactsInfo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = contactsInfo.ID }, contactsInfo);

            
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var message = await _context.Messages
                            .Include(i => i.MessageBoard)
                            .FirstOrDefaultAsync(i => i.Id == message.Id);

            return Json(message);
        }

        // DELETE: api/Contacts/5
        [ResponseType(typeof(ContactsInfo))]
        public IHttpActionResult DeleteContactsInfo(int id)
        {
            ContactsInfo contactsInfo = db.ContactsInfos.Find(id);
            if (contactsInfo == null)
            {
                return NotFound();
            }

            db.ContactsInfos.Remove(contactsInfo);
            db.SaveChanges();

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