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
    [Authorize]
    public class CarrosController : ApiController
    {
        private BDPRUEBASEntities db = new BDPRUEBASEntities();

        // GET: api/Carros
        public IQueryable<Carro> GetCarro()
        {
            return db.Carro.Take(100);
        }

        // GET: api/Carros/5
        [ResponseType(typeof(Carro))]
        public IHttpActionResult GetCarro(string id)
        {
            Carro carro = db.Carro.Find(id);
            if (carro == null)
            {
                return NotFound();
            }

            return Ok(carro);
        }

        // PUT: api/Carros/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCarro(string id, Carro carro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carro.PLACA)
            {
                return BadRequest();
            }

            db.Entry(carro).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarroExists(id))
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

        // POST: api/Carros
        [ResponseType(typeof(Carro))]
        public IHttpActionResult PostCarro(Carro carro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Carro.Add(carro);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CarroExists(carro.PLACA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = carro.PLACA }, carro);
        }

        // DELETE: api/Carros/5
        [ResponseType(typeof(Carro))]
        public IHttpActionResult DeleteCarro(string id)
        {
            Carro carro = db.Carro.Find(id);
            if (carro == null)
            {
                return NotFound();
            }

            db.Carro.Remove(carro);
            db.SaveChanges();

            return Ok(carro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarroExists(string id)
        {
            return db.Carro.Count(e => e.PLACA == id) > 0;
        }
    }
}