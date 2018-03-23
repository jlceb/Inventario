using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MyStock.Domain;

namespace MyStock.API.Controllers
{
    [Authorize]
    public class UbicationsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Ubications
        public IQueryable<Ubication> GetUbications()
        {
            return db.Ubications;
        }

        // GET: api/Ubications/5
        [ResponseType(typeof(Ubication))]
        public async Task<IHttpActionResult> GetUbications(int id)
        {
            Ubication ubications = await db.Ubications.FindAsync(id);
            if (ubications == null)
            {
                return NotFound();
            }

            return Ok(ubications);
        }

        // PUT: api/Ubications/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUbications(int id, Ubication ubications)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ubications.UbicationId)
            {
                return BadRequest();
            }

            db.Entry(ubications).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UbicationsExists(id))
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

        // POST: api/Ubications
        [ResponseType(typeof(Ubication))]
        public async Task<IHttpActionResult> PostUbications(Ubication ubications)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ubications.Add(ubications);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ubications.UbicationId }, ubications);
        }

        // DELETE: api/Ubications/5
        [ResponseType(typeof(Ubication))]
        public async Task<IHttpActionResult> DeleteUbications(int id)
        {
            Ubication ubications = await db.Ubications.FindAsync(id);
            if (ubications == null)
            {
                return NotFound();
            }

            db.Ubications.Remove(ubications);
            await db.SaveChangesAsync();

            return Ok(ubications);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UbicationsExists(int id)
        {
            return db.Ubications.Count(e => e.UbicationId == id) > 0;
        }
    }
}