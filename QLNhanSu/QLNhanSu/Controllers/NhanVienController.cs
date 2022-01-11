using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLNhanSu.Models;
using System.IO;
using PagedList;

namespace QLNhanSu.Controllers
{
    public class NhanVienController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: NhanVien
        public ActionResult Index(int? page)
        {
            var nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).OrderBy(n => n.MANV);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(nhanViens.ToPagedList(pageNumber, pageSize));
        }

        // GET: NhanVien/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanVien/Create
        public ActionResult Create()
        {
            ViewBag.MACV = new SelectList(db.ChucVus, "MACV", "TENCV");
            ViewBag.MAPB = new SelectList(db.PhongBans, "MAPB", "TENPB");
            ViewBag.MATDHV = new SelectList(db.TrinhDoHocVans, "MATDHV", "TENTRINHDO");
            return View();
        }

        // POST: NhanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANV,HOTEN,ANHDAIDIEN,DANTOC,GIOITINH,SODIENTHOAI,QUEQUAN,NGAYSINH,MACV,MATDHV,MAPB,NGAYBATDAU,CHUYENNGANH")] NhanVien nhanVien, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    string filename = Path.GetFileName(Image.FileName);
                    string path = Server.MapPath("~/Images/" + filename);
                    nhanVien.ANHDAIDIEN = "Images/" + filename;
                    Image.SaveAs(path);
                }
                DateTime today = DateTime.Today;
                nhanVien.NGAYBATDAU = today;
                Luong luong = new Luong();
                luong.MANV = nhanVien.MANV;
                luong.LUONGCOBAN = 0;
                luong.HESOLUONG = 0;
                luong.PHUCAP = 0;
                db.Luongs.Add(luong);
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MACV = new SelectList(db.ChucVus, "MACV", "TENCV", nhanVien.MACV);
            ViewBag.MAPB = new SelectList(db.PhongBans, "MAPB", "TENPB", nhanVien.MAPB);
            ViewBag.MATDHV = new SelectList(db.TrinhDoHocVans, "MATDHV", "TENTRINHDO", nhanVien.MATDHV);
            return View(nhanVien);
        }

        // GET: NhanVien/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MACV = new SelectList(db.ChucVus, "MACV", "TENCV", nhanVien.MACV);
            ViewBag.MAPB = new SelectList(db.PhongBans, "MAPB", "TENPB", nhanVien.MAPB);
            ViewBag.MATDHV = new SelectList(db.TrinhDoHocVans, "MATDHV", "TENTRINHDO", nhanVien.MATDHV);
            return View(nhanVien);
        }

        // POST: NhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANV,HOTEN,ANHDAIDIEN,DANTOC,GIOITINH,SODIENTHOAI,QUEQUAN,NGAYSINH,MACV,MATDHV,MAPB,NGAYBATDAU,CHUYENNGANH")] NhanVien nhanVien, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    if (nhanVien.ANHDAIDIEN != null)
                        System.IO.File.Delete(Server.MapPath("~/" + nhanVien.ANHDAIDIEN));
                    string filename = Path.GetFileName(Image.FileName);
                    string path = Server.MapPath("~/Images/" + filename);
                    nhanVien.ANHDAIDIEN = "Images/" + filename;
                    Image.SaveAs(path);
                }
                db.Entry(nhanVien).State = EntityState.Modified;
                nhanVien.MAPB = nhanVien.MAPB;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MACV = new SelectList(db.ChucVus, "MACV", "TENCV", nhanVien.MACV);
            ViewBag.MAPB = new SelectList(db.PhongBans, "MAPB", "TENPB", nhanVien.MAPB);
            ViewBag.MATDHV = new SelectList(db.TrinhDoHocVans, "MATDHV", "TENTRINHDO", nhanVien.MATDHV);
            return View(nhanVien);
        }

        // GET: NhanVien/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            
            db.NhanViens.Remove(nhanVien);      
            db.SaveChanges();
            if (nhanVien.ANHDAIDIEN != null)
                System.IO.File.Delete(Server.MapPath("~/" + nhanVien.ANHDAIDIEN));
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
