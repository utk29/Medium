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
    public class searchController : ApiController
    {
        private RegistrationEntities2 db = new RegistrationEntities2();

        // GET: api/search
        public IQueryable<blog> Getblogs()
        {
            return db.blogs;
        }

        // GET: api/search/5
        [ResponseType(typeof(blog))]
        public IQueryable<blog> Getblog(string id)
        {
            return (from b in db.blogs where b.BlogTitle.Contains(id) select b);
            
        }

        // PUT: api/search/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putblog(string id, blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blog.UserId)
            {
                return BadRequest();
            }

            db.Entry(blog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!blogExists(id))
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

        // POST: api/search
        [ResponseType(typeof(blog))]
        public IHttpActionResult Postblog(blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.blogs.Add(blog);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (blogExists(blog.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = blog.UserId }, blog);
        }

        // DELETE: api/search/5
        [ResponseType(typeof(blog))]
        public IHttpActionResult Deleteblog(string id)
        {
            blog blog = db.blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            db.blogs.Remove(blog);
            db.SaveChanges();

            return Ok(blog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool blogExists(string id)
        {
            return db.blogs.Count(e => e.UserId == id) > 0;
        }
    }
}