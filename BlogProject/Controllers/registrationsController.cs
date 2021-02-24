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
using BlogProject.Models;

namespace BlogProject.Controllers
{
    public class registrationsController : ApiController
    {
        private RegistrationEntities db = new RegistrationEntities();

        // GET: api/registrations
        public IQueryable<registration> Getregistrations()
        {
            return db.registrations;
        }
        

        // GET: api/registrations/5
        [ResponseType(typeof(registration))]
        [HttpGet]
        public IHttpActionResult Getregistration(string id)
        {
            registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return NotFound();
            }

            return Ok(registration);
        }

        // PUT: api/registrations/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult Putregistration(string id, registration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registration.user_id)
            {
                return BadRequest();
            }

            db.update_profile(id,registration.user_id, registration.first_name, registration.middle_name, registration.last_name,registration.phone_number
                ,registration.email_id,registration.pass,registration.photo_filename, registration.bio);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/registrations
        [ResponseType(typeof(registration))]
        [HttpPost]
        public IHttpActionResult Postregistration(registration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userdata = db.user_signup(registration.user_id, registration.first_name, registration.middle_name, registration.last_name,
                registration.phone_number, registration.email_id, registration.gender, registration.date_of_birth, registration.pass,
                registration.photo_filename, registration.bio);

            return CreatedAtRoute("DefaultApi", new { id = registration.user_id }, registration);
        }

        // DELETE: api/registrations/5
        [ResponseType(typeof(registration))]
        public IHttpActionResult Deleteregistration(string id)
        {
            registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return NotFound();
            }

            db.registrations.Remove(registration);
            db.SaveChanges();

            return Ok(registration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool registrationExists(string id)
        {
            return db.registrations.Count(e => e.user_id == id) > 0;
        }

        [HttpGet]
        public user_login_Result user_login_Results(string username, string password)
        {
            user_login_Result useractive;
            useractive = db.user_login(username, password).FirstOrDefault();
            return useractive;
        }
    }
}