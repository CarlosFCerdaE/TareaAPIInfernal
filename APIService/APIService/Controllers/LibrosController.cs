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
using APIService.Models;

namespace APIService.Controllers
{
    public class LibrosController : ApiController
    {
        private BDPRUEBASEntities db = new BDPRUEBASEntities();

        // GET: api/Libros
        public IQueryable<Libro> GetLibro()
        {
            return db.Libro;
        }

        // GET: api/Libros/5
        [ResponseType(typeof(Libro))]
        public IHttpActionResult GetLibro(string id)
        {
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return NotFound();
            }

            return Ok(libro);
        }

        // PUT: api/Libros/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLibro(string id, Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != libro.ISBN)
            {
                return BadRequest();
            }

            db.Entry(libro).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
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

        // POST: api/Libros
        [ResponseType(typeof(Libro))]
        public IHttpActionResult PostLibro(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Libro.Add(libro);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LibroExists(libro.ISBN))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = libro.ISBN }, libro);
        }

        // DELETE: api/Libros/5
        [ResponseType(typeof(Libro))]
        public IHttpActionResult DeleteLibro(string id)
        {
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return NotFound();
            }

            db.Libro.Remove(libro);
            db.SaveChanges();

            return Ok(libro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LibroExists(string id)
        {
            return db.Libro.Count(e => e.ISBN == id) > 0;
        }
    }
}