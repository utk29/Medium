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
    public class blog_likesController : ApiController
    {
        private RegistrationEntities3 db = new RegistrationEntities3();

        // GET: api/blog_likes
        public IQueryable<blog_likes> Getblog_likes()
        {
            return db.blog_likes;
        }





        // GET: api/blog_likes/5
        [ResponseType(typeof(blog_likes))]
        public IQueryable<blog_likes> Getblog_likes(string id)
        {

            return from b in db.blog_likes where b.LikerId == id select b;
        }

        // PUT: api/blog_likes/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult Putblog_likes(string uid,int bid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.like_count(uid, bid);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!blog_likesExists(uid))
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

        // POST: api/blog_likes
        [ResponseType(typeof(blog_likes))]
        [HttpPost]
        public string Postblog_likes(string uid, int bid,string lid)
        {
            if (!ModelState.IsValid)
            {
                return ("BadRequest");
            }

            db.blog_like_insert(uid, bid, lid);

           
            return ("successful");
        }

        // DELETE: api/blog_likes/5
        [ResponseType(typeof(blog_likes))]
        [HttpDelete]
        public IHttpActionResult Deleteblog_likes(string uid, int bid, string lid)
        {
            db.dislike(uid,bid,lid);
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

        private bool blog_likesExists(string id)
        {
            return db.blog_likes.Count(e => e.UserId == id) > 0;
        }
    }
}