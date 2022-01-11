using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLNhanSu.Models;

namespace QLNhanSu.Controllers
{
    public class TrinhDoHocVanController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: TrinhDoHocVan
        public ActionResult Index()
        { 
            return View(db.TrinhDoHocVans.ToList());
        }

        // GET: TrinhDoHocVan/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoHocVan trinhDoHocVan = db.TrinhDoHocVans.Find(id);
            if (trinhDoHocVan == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoHocVan);
        }

        // GET: TrinhDoHocVan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrinhDoHocVan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MATDHV,TENTRINHDO,CHUYENNGANH")] TrinhDoHocVan trinhDoHocVan)
        {
            if (ModelState.IsValid)
            {
                db.TrinhDoHocVans.Add(trinhDoHocVan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trinhDoHocVan);
        }

        // GET: TrinhDoHocVan/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoHocVan trinhDoHocVan = db.TrinhDoHocVans.Find(id);
            if (trinhDoHocVan == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoHocVan);
        }

        // POST: TrinhDoHocVan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MATDHV,TENTRINHDO,CHUYENNGANH")] TrinhDoHocVan trinhDoHocVan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trinhDoHocVan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trinhDoHocVan);
        }

        // GET: TrinhDoHocVan/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoHocVan trinhDoHocVan = db.TrinhDoHocVans.Find(id);
            if (trinhDoHocVan == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoHocVan);
        }

        // POST: TrinhDoHocVan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TrinhDoHocVan trinhDoHocVan = db.TrinhDoHocVans.Find(id);
            db.TrinhDoHocVans.Remove(trinhDoHocVan);
            db.SaveChanges();
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
