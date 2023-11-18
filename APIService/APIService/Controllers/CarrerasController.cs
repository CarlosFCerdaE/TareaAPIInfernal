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
    public class CarrerasController : ApiController
    {
        private BDPRUEBASEntities db = new BDPRUEBASEntities();

        // GET: api/Carreras
        public IQueryable<Carrera> GetCarrera()
        {
            return db.Carrera.Take(100);
        }

        // GET: api/Carreras/5
        [ResponseType(typeof(Carrera))]
        public IHttpActionResult GetCarrera(string id)
        {
            Carrera carrera = db.Carrera.Find(id);
            if (carrera == null)
            {
                return NotFound();
            }

            return Ok(carrera);
        }

        // PUT: api/Carreras/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCarrera(string id, Carrera carrera)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carrera.ID)
            {
                return BadRequest();
            }

            db.Entry(carrera).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarreraExists(id))
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

        // POST: api/Carreras
        [ResponseType(typeof(Carrera))]
        public IHttpActionResult PostCarrera(Carrera carrera)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Carrera.Add(carrera);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CarreraExists(carrera.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = carrera.ID }, carrera);
        }

        // DELETE: api/Carreras/5
        [ResponseType(typeof(Carrera))]
        public IHttpActionResult DeleteCarrera(string id)
        {
            Carrera carrera = db.Carrera.Find(id);
            if (carrera == null)
            {
                return NotFound();
            }

            db.Carrera.Remove(carrera);
            db.SaveChanges();

            return Ok(carrera);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarreraExists(string id)
        {
            return db.Carrera.Count(e => e.ID == id) > 0;
        }
    }
}