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
using ClosedXML.Excel;
using System.IO;

namespace QLNhanSu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PhongBanController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: PhongBan
        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var phongBans = db.PhongBans.OrderBy(p => p.MAPB);
            if(searchString != null && searchString != "")
            {
                phongBans = db.PhongBans.Where(p => p.MAPB.Contains(searchString) || p.TENPB.Contains(searchString)).OrderBy(p => p.MAPB);
            }
            return View(phongBans.ToPagedList(pageNumber, pageSize));

        }

        // GET: PhongBan/Details/5
        public ActionResult Details(string id, int? page, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = id;
            ViewBag.searchString = searchString;
            var nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.MAPB == id).OrderBy(n => n.MANV);
            if(String.IsNullOrEmpty(searchString) == false)
                nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.MAPB == id && (n.HOTEN.Contains(searchString) || n.MANV.Contains(searchString))).OrderBy(n => n.MANV);
            ViewBag.PBName = db.PhongBans.Find(id).TENPB;
            //PhongBan phongBan = db.PhongBans.Find(id);
            //if (phongBan == null)
            //{
            //    return HttpNotFound();
            //}
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(nhanViens.ToPagedList(pageNumber, pageSize));
        }

        // GET: PhongBan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhongBan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAPB,TENPB,DIACHI,SODIENTHOAI")] PhongBan phongBan)
        {
            if (ModelState.IsValid)
            {
                db.PhongBans.Add(phongBan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phongBan);
        }

        // GET: PhongBan/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongBan phongBan = db.PhongBans.Find(id);
            if (phongBan == null)
            {
                return HttpNotFound();
            }
            return View(phongBan);
        }

        // POST: PhongBan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAPB,TENPB,DIACHI,SODIENTHOAI")] PhongBan phongBan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phongBan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phongBan);
        }

        // GET: PhongBan/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongBan phongBan = db.PhongBans.Find(id);
            if (phongBan == null)
            {
                return HttpNotFound();
            }
            return View(phongBan);
        }

        // POST: PhongBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PhongBan phongBan = db.PhongBans.Find(id);
            List<ChuyenPhongBan> chuyenphongban = db.ChuyenPhongBans.Where(c => c.MAPBCHUYENDEN == id).ToList();

            foreach(var item in chuyenphongban)
            {
                db.ChuyenPhongBans.Remove(item);
                db.SaveChanges();
            }
            db.PhongBans.Remove(phongBan);
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
        [HttpPost]
        public FileResult Export(string searchString, string id)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] {
                new DataColumn("Mã nhân viên"),
                new DataColumn("Họ tên"),
                new DataColumn("Giới tính"),
                new DataColumn("Số điện thoại"),
                new DataColumn("Chức vụ"),
                new DataColumn("Học vấn"),
                new DataColumn("Chuyên ngành"),
                });
            var nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.MAPB == id).OrderBy(n => n.MANV).ToList();
            if (String.IsNullOrEmpty(searchString) == false)
                nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.MAPB == id && (n.HOTEN.Contains(searchString) || n.MANV.Contains(searchString))).OrderBy(n => n.MANV).ToList();
            foreach (var nv in nhanViens)
            {
                var chucvu = "";
                var trinhdohocvan = "";
                if (nv.ChucVu != null)
                {
                    ChucVu cv = db.ChucVus.Find(nv.MACV);
                    chucvu = cv.TENCV;
                }
                if (nv.TrinhDoHocVan != null)
                {
                    TrinhDoHocVan tdhv = db.TrinhDoHocVans.Find(nv.MATDHV);
                    trinhdohocvan = tdhv.TENTRINHDO;
                }
                dt.Rows.Add(nv.MANV, nv.HOTEN, nv.GIOITINH, nv.SODIENTHOAI, chucvu, trinhdohocvan, nv.CHUYENNGANH);
            }
            PhongBan pb = db.PhongBans.Find(id);
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NhanVienPB "+pb?.TENPB+".xlsx");
                }
            }
        }
    }
}
