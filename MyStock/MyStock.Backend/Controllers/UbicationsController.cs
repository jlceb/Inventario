using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyStock.Backend.Models;
using MyStock.Domain;

namespace MyStock.Backend.Controllers
{
    [Authorize]
    public class UbicationsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Ubications
        public async Task<ActionResult> Index()
        {
            return View(await db.Ubications.ToListAsync());
        }

        // GET: Ubications/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubication ubications = await db.Ubications.FindAsync(id);
            if (ubications == null)
            {
                return HttpNotFound();
            }
            return View(ubications);
        }

        // GET: Ubications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ubications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UbicationId,Description,Address,Phone,Latitude,Longitude")] Ubication ubications)
        {
            if (ModelState.IsValid)
            {
                db.Ubications.Add(ubications);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ubications);
        }

        // GET: Ubications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubication ubications = await db.Ubications.FindAsync(id);
            if (ubications == null)
            {
                return HttpNotFound();
            }
            return View(ubications);
        }

        // POST: Ubications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UbicationId,Description,Address,Phone,Latitude,Longitude")] Ubication ubications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ubications).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ubications);
        }

        // GET: Ubications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubication ubications = await db.Ubications.FindAsync(id);
            if (ubications == null)
            {
                return HttpNotFound();
            }
            return View(ubications);
        }

        // POST: Ubications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ubication ubications = await db.Ubications.FindAsync(id);
            db.Ubications.Remove(ubications);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
