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
    public class reviewsController : ApiController
    {
        private RegistrationEntities5 db = new RegistrationEntities5();

        // GET: api/reviews
        public IQueryable<review> Getreviews()
        {
            return db.reviews;
        }

        // GET: api/reviews/5
        [ResponseType(typeof(review))]
        public IQueryable<review> Getreview(string id)
        {
            return (from r in db.reviews where r.UserId == id select r);
            //review review = db.reviews.Find(id);
            //if (review == null)
            //{
            //    return NotFound();
            //}

            //return Ok(review);
        }

        // PUT: api/reviews/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult Putreview(string id, review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.UserId)
            {
                return BadRequest();
            }

            db.Entry(review).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!reviewExists(id))
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

        // POST: api/reviews
        [ResponseType(typeof(review))]
        public IHttpActionResult Postreview(review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.reviews.Add(review);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (reviewExists(review.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = review.UserId }, review);
        }

        // DELETE: api/reviews/5
        [ResponseType(typeof(review))]
        public IHttpActionResult Deletereview(string id)
        {
            review review = db.reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            db.reviews.Remove(review);
            db.SaveChanges();

            return Ok(review);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool reviewExists(string id)
        {
            return db.reviews.Count(e => e.UserId == id) > 0;
        }
    }
}