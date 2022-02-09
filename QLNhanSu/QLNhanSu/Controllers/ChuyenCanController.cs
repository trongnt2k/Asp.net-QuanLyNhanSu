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
    public class ChuyenCanController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: ChuyenCan
        public ActionResult Index(string id, int? page)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var chuyenCans = db.ChuyenCans.Include(c => c.NhanVien).OrderBy(c => c.MACC).Where(c => c.MANV == id);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(chuyenCans.ToPagedList(pageNumber, pageSize));
        }

        // GET: ChuyenCan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenCan chuyenCan = db.ChuyenCans.Find(id);
            if (chuyenCan == null)
            {
                return HttpNotFound();
            }
            return View(chuyenCan);
        }

        // GET: ChuyenCan/Create
        public ActionResult Create(string id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.nhanVien = nhanVien;
            ChuyenCan chuyencan = new ChuyenCan();
            chuyencan.NGAYVANG = DateTime.Today;
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN");
            return View(chuyencan);
        }

        // POST: ChuyenCan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MACC,MANV,LYDO,NGAYVANG")] ChuyenCan chuyenCan)
        {
            if (ModelState.IsValid)
            {
                db.ChuyenCans.Add(chuyenCan);
                db.SaveChanges();
                return RedirectToAction("Index", "Luong");
            }

            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", chuyenCan.MANV);
            return View(chuyenCan);
        }

        // GET: ChuyenCan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenCan chuyenCan = db.ChuyenCans.Find(id);
            if (chuyenCan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", chuyenCan.MANV);
            return View(chuyenCan);
        }

        // POST: ChuyenCan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MACC,MANV,LYDO,NGAYVANG")] ChuyenCan chuyenCan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenCan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", chuyenCan.MANV);
            return View(chuyenCan);
        }

        // GET: ChuyenCan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenCan chuyenCan = db.ChuyenCans.Find(id);
            if (chuyenCan == null)
            {
                return HttpNotFound();
            }
            return View(chuyenCan);
        }

        // POST: ChuyenCan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChuyenCan chuyenCan = db.ChuyenCans.Find(id);
            db.ChuyenCans.Remove(chuyenCan);
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
