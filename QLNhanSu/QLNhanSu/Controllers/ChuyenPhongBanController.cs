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
    public class ChuyenPhongBanController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: ChuyenPhongBan
        public ActionResult Index(string id, int? page)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var chuyenPhongBans = db.ChuyenPhongBans.Include(c => c.NhanVien).Include(c => c.PhongBan).Include(c => c.PhongBan1).Where(c => c.MANV == id).OrderBy(c => c.MACPB);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(chuyenPhongBans.ToPagedList(pageNumber, pageSize));
        }

        // GET: ChuyenPhongBan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenPhongBan chuyenPhongBan = db.ChuyenPhongBans.Find(id);
            if (chuyenPhongBan == null)
            {
                return HttpNotFound();
            }
            return View(chuyenPhongBan);
        }

        // GET: ChuyenPhongBan/Create
        public ActionResult Create(string id)
        {
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN");
            ViewBag.MAPB = new SelectList(db.PhongBans, "MAPB", "TENPB");
            ViewBag.MAPBCHUYENDEN = new SelectList(db.PhongBans, "MAPB", "TENPB");
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.nhanVien = nhanVien;
            return View();
        }

        // POST: ChuyenPhongBan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MACPB,MANV,MAPB,MAPBCHUYENDEN,LYDO")] ChuyenPhongBan chuyenPhongBan)
        {
            if (ModelState.IsValid)
            {
                db.ChuyenPhongBans.Add(chuyenPhongBan);
                NhanVien nhanVien = db.NhanViens.Find(chuyenPhongBan.MANV);
                nhanVien.MAPB = chuyenPhongBan.MAPBCHUYENDEN;
                DateTime today = DateTime.Today;
                chuyenPhongBan.NGAYCHUYEN = today;
                db.SaveChanges();
                return RedirectToAction("Index", "PhongBan");
            }

            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", chuyenPhongBan.MANV);
            ViewBag.MAPB = new SelectList(db.PhongBans, "MAPB", "TENPB", chuyenPhongBan.MAPB);
            ViewBag.MAPBCHUYENDEN = new SelectList(db.PhongBans, "MAPB", "TENPB", chuyenPhongBan.MAPBCHUYENDEN);
            return View(chuyenPhongBan);
        }

        // GET: ChuyenPhongBan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenPhongBan chuyenPhongBan = db.ChuyenPhongBans.Find(id);
            if (chuyenPhongBan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", chuyenPhongBan.MANV);
            ViewBag.MAPB = new SelectList(db.PhongBans, "MAPB", "TENPB", chuyenPhongBan.MAPB);
            ViewBag.MAPBCHUYENDEN = new SelectList(db.PhongBans, "MAPB", "TENPB", chuyenPhongBan.MAPBCHUYENDEN);
            return View(chuyenPhongBan);
        }

        // POST: ChuyenPhongBan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MACPB,MANV,MAPB,MAPBCHUYENDEN,LYDO")] ChuyenPhongBan chuyenPhongBan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenPhongBan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", chuyenPhongBan.MANV);
            ViewBag.MAPB = new SelectList(db.PhongBans, "MAPB", "TENPB", chuyenPhongBan.MAPB);
            ViewBag.MAPBCHUYENDEN = new SelectList(db.PhongBans, "MAPB", "TENPB", chuyenPhongBan.MAPBCHUYENDEN);
            return View(chuyenPhongBan);
        }

        // GET: ChuyenPhongBan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenPhongBan chuyenPhongBan = db.ChuyenPhongBans.Find(id);
            if (chuyenPhongBan == null)
            {
                return HttpNotFound();
            }
            return View(chuyenPhongBan);
        }

        // POST: ChuyenPhongBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChuyenPhongBan chuyenPhongBan = db.ChuyenPhongBans.Find(id);
            db.ChuyenPhongBans.Remove(chuyenPhongBan);
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
