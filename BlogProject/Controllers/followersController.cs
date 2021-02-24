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
    public class followersController : ApiController
    {
        private RegistrationEntities6 db = new RegistrationEntities6();

        // GET: api/followers
        public IQueryable<follower> Getfollowers()
        {
            return db.followers;
        }

        // GET: api/followers/5
        [ResponseType(typeof(follower))]
        [HttpGet]
        public IQueryable<follower> Getfollower(string uid, string fid)
        {
            return from b in db.followers where b.UserId == uid  select b; 
        }

        // PUT: api/followers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putfollower(string id, follower follower)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != follower.UserId)
            {
                return BadRequest();
            }

            db.Entry(follower).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!followerExists(id))
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

        // POST: api/followers
        [ResponseType(typeof(follower))]
        public IHttpActionResult Postfollower(follower follower)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.followers.Add(follower);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (followerExists(follower.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = follower.UserId }, follower);
        }

        // DELETE: api/followers/5
        [ResponseType(typeof(follower))]
        [HttpDelete]
        public string Deletefollower(string uid, string fid)
        {
            int follower = db.delete_follower(uid,fid);
            if (follower == 0)
            {
                return ("not found");
            }
            db.SaveChanges();

            return ("deleted");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool followerExists(string id)
        {
            return db.followers.Count(e => e.UserId == id) > 0;
        }
    }
}