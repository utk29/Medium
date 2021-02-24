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
    public class blogsController : ApiController
    {
        private RegistrationEntities2 db = new RegistrationEntities2();

        // GET: api/blogs
        public IQueryable<blog> Getblogs()
        {
            return db.blogs;
        }

        // GET: api/blogs/5
        [ResponseType(typeof(blog))]
        [HttpGet]
        public IHttpActionResult Getblog(string id)
        {
            var blog = db.get_blogs(id);
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }

        

        [HttpGet]
        public IHttpActionResult Getblog(string uid, int bid)
        {
            var blog = db.get_blog_for_update(uid, bid);
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }

        // PUT: api/blogs/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult Putblog(string id, int bid, blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blog.UserId)
            {
                return BadRequest();
            }

            db.update_blog(id, bid, blog.BlogTitle, blog.BlogImage, blog.BlogContent, blog.Blogtype);

            return StatusCode(HttpStatusCode.OK);
        }

       

        // POST: api/blogs
        [ResponseType(typeof(blog))]
        [HttpPost]
        public IHttpActionResult Postblog(blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             db.blog_insert(blog.UserId, blog.BlogTitle, blog.BlogImage, blog.BlogContent, blog.Blogtype);
            //db.blogs.Add(blog);
            //db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = blog.UserId }, blog);
        }

        

        // DELETE: api/blogs/5
        [ResponseType(typeof(blog))]

        public IHttpActionResult Deleteblog(string uid, int bid)
        {
            int i = db.delete_blog(uid,bid);
            if (i == 0)
            {
                return NotFound();
            }

            
            db.SaveChanges();

            return Ok();
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