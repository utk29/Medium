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
    public class SearchFollowerController : ApiController
    {
        private RegistrationEntities db = new RegistrationEntities();

        // GET: api/SearchFollower
        public IQueryable<registration> Getregistrations()
        {
            return db.registrations;
        }

        // GET: api/SearchFollower/5
        [ResponseType(typeof(registration))]
        public IQueryable<registration> Getregistration(string id)
        {
            return (from r in db.registrations where r.user_id.Contains(id) select r);
        }

        // PUT: api/SearchFollower/5
        [ResponseType(typeof(void))]
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

            db.Entry(registration).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!registrationExists(id))
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

        // POST: api/SearchFollower
        [ResponseType(typeof(registration))]
        public IHttpActionResult Postregistration(registration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.registrations.Add(registration);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (registrationExists(registration.user_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = registration.user_id }, registration);
        }

        // DELETE: api/SearchFollower/5
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
    }
}