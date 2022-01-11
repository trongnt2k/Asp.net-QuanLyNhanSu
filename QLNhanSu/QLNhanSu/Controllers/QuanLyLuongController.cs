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
    public class QuanLyLuongController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: QuanLyLuong
        public ActionResult Index(string id, int? page)
        {
            var quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).OrderBy(q => q.MANV);
            if (id != null)
            {
                quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id).OrderBy(q => q.MANV);
            }
            else
            {
                quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).OrderBy(q => q.MANV);
            }    
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(quanLyLuongs.ToPagedList(pageNumber, pageSize));
        }

        // GET: QuanLyLuong/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanLyLuong quanLyLuong = db.QuanLyLuongs.Find(id);
            if (quanLyLuong == null)
            {
                return HttpNotFound();
            }
            return View(quanLyLuong);
        }

        // GET: QuanLyLuong/Create
        public ActionResult Create(string id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            Luong luong = db.Luongs.Find(id);
            ViewBag.nhanVien = nhanVien;
            ViewBag.luong = luong;
            QuanLyLuong quanlyluong = new QuanLyLuong();
            quanlyluong.NGAYTINHLUONG = DateTime.Today;
            var ngayvang = db.ChuyenCans.Where(c => c.MANV == id).Count();
            ViewBag.NgayCong = 26 - ngayvang;
            ViewBag.THUCLANH = (luong.LUONGCOBAN * luong.HESOLUONG + luong.PHUCAP)/26 * (26 - ngayvang);
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN");

            return View(quanlyluong);
        }

        // POST: QuanLyLuong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAQLL,MANV,NGAYTINHLUONG,THUCLANH")] QuanLyLuong quanLyLuong)
        {
            if (ModelState.IsValid)
            {
                Luong luong = db.Luongs.Find(quanLyLuong.MANV);
                quanLyLuong.LUONGCOBAN = luong.LUONGCOBAN;
                db.QuanLyLuongs.Add(quanLyLuong);
                db.SaveChanges();
                return RedirectToAction("Index", "Luong");
            }

            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", quanLyLuong.MANV);
            return View(quanLyLuong);
        }

        public ActionResult CreateAll()
        {
            List<NhanVien> nv = db.NhanViens.ToList();
            foreach(var item in nv)
            {
                Luong luong = db.Luongs.Find(item.MANV);
                QuanLyLuong quanlyluong = new QuanLyLuong();
                quanlyluong.NGAYTINHLUONG = DateTime.Today;
                quanlyluong.LUONGCOBAN = luong.LUONGCOBAN;
                quanlyluong.MANV = item.MANV;
                var ngayvang = db.ChuyenCans.Where(c => c.MANV == item.MANV).Count();
                quanlyluong.THUCLANH = (luong.LUONGCOBAN * luong.HESOLUONG + luong.PHUCAP) / 26 * (26 - ngayvang);
                db.QuanLyLuongs.Add(quanlyluong);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "QuanLyLuong");
        }

        // GET: QuanLyLuong/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanLyLuong quanLyLuong = db.QuanLyLuongs.Find(id);
            if (quanLyLuong == null)
            {
                return HttpNotFound();
            }
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", quanLyLuong.MANV);
            return View(quanLyLuong);
        }

        // POST: QuanLyLuong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAQLL,MANV,NGAYTINHLUONG,THUCLANH")] QuanLyLuong quanLyLuong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quanLyLuong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANV = new SelectList(db.NhanViens, "MANV", "HOTEN", quanLyLuong.MANV);
            return View(quanLyLuong);
        }

        // GET: QuanLyLuong/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanLyLuong quanLyLuong = db.QuanLyLuongs.Find(id);
            if (quanLyLuong == null)
            {
                return HttpNotFound();
            }
            return View(quanLyLuong);
        }

        // POST: QuanLyLuong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuanLyLuong quanLyLuong = db.QuanLyLuongs.Find(id);
            db.QuanLyLuongs.Remove(quanLyLuong);
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
