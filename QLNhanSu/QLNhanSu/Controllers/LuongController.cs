using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLNhanSu.Models;
using PagedList;

namespace QLNhanSu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LuongController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: Luong
        public ActionResult Index(int? page, string searchString)
        {
            var luongs = db.Luongs.Include(l => l.NhanVien).OrderBy(l => l.MANV);
            if(searchString != null && searchString != "")
            {
                luongs = db.Luongs.Include(l => l.NhanVien).Where(l => l.MANV.Contains(searchString) || l.NhanVien.HOTEN.Contains(searchString)).OrderBy(l => l.MANV);
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(luongs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Luong/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luong luong = db.Luongs.Find(id);
            if (luong == null)
            {
                return HttpNotFound();
            }
            return View(luong);
        }

        // GET: Luong/Create
        public ActionResult Create()
        {
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN");
            return View();
        }

        // POST: Luong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANV,LUONGCOBAN,HESOLUONG,PHUCAP")] Luong luong)
        {
            if (ModelState.IsValid)
            {
                db.Luongs.Add(luong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", luong.MANV);
            return View(luong);
        }

        // GET: Luong/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luong luong = db.Luongs.Find(id);
            if (luong == null)
            {
                return HttpNotFound();
            }
            ViewBag.MNV = luong.MANV;
            ViewBag.TNV = luong.NhanVien.HOTEN;
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", luong.MANV);
            return View(luong);
        }

        // POST: Luong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANV,LUONGCOBAN,HESOLUONG,PHUCAP")] Luong luong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(luong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", luong.MANV);
            return View(luong);
        }

        // GET: Luong/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luong luong = db.Luongs.Find(id);
            if (luong == null)
            {
                return HttpNotFound();
            }
            return View(luong);
        }

        // POST: Luong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Luong luong = db.Luongs.Find(id);
            db.Luongs.Remove(luong);
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
