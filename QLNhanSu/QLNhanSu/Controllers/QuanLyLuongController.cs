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
using System.Globalization;
using System.IO;

namespace QLNhanSu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuanLyLuongController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: QuanLyLuong
        public ActionResult Index(string id, int? page, string searchString, string fromdate, string todate)
        {
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.searchString = searchString;
            ViewBag.id = id;
            var quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).OrderBy(q => q.MANV);
            if (id != null)
            {
                if (searchString != null)
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id && (q.NhanVien.HOTEN.Contains(searchString)) && (q.NGAYTINHLUONG >= fd && q.NGAYTINHLUONG <= td)).OrderBy(q => q.MAQLL);
                    }
                    if (fromdate != "" && todate == "")
                    {
                        DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id && (q.NhanVien.HOTEN.Contains(searchString)) && q.NGAYTINHLUONG >= fd).OrderBy(q => q.MAQLL);
                    }
                    if (fromdate == "" && todate != "")
                    {
                        DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id && (q.NhanVien.HOTEN.Contains(searchString)) && q.NGAYTINHLUONG >= td).OrderBy(q => q.MAQLL);
                    }
                    if (fromdate == "" && todate == "")
                    {
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id && q.NhanVien.HOTEN.Contains(searchString)).OrderBy(q => q.MAQLL);
                    }
                }
                else
                    quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id).OrderBy(q => q.MAQLL);
            }
            if (id == null)
            {
                if (searchString != null)
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.NhanVien.HOTEN.Contains(searchString) && (q.NGAYTINHLUONG >= fd && q.NGAYTINHLUONG <= td)).OrderBy(q => q.MAQLL);
                    }
                    if (fromdate != "" && todate == "")
                    {
                        DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.NhanVien.HOTEN.Contains(searchString) && q.NGAYTINHLUONG >= fd).OrderBy(q => q.MAQLL);
                    }
                    if (fromdate == "" && todate != "")
                    {
                        DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.NhanVien.HOTEN.Contains(searchString) && q.NGAYTINHLUONG >= td).OrderBy(q => q.MAQLL);
                    }
                    if (fromdate == "" && todate == "")
                    {
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.NhanVien.HOTEN.Contains(searchString)).OrderBy(q => q.MAQLL);
                    }
                }
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

        [HttpPost]
        public FileResult Export(string searchString, string fromdate, string todate, string id)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6] {
                new DataColumn("Mã tính lương"),
                new DataColumn("Mã nhân viên"),
                new DataColumn("Họ tên"),
                new DataColumn("Lương cơ bản"),
                new DataColumn("Thực lãnh"),
                new DataColumn("Ngày tính lương"),
                });
            var quanLyLuongs = db.QuanLyLuongs.OrderBy(n => n.MAQLL).ToList();
            if (String.IsNullOrEmpty(id) == false)
            {
                if (String.IsNullOrEmpty(searchString) == false || searchString == "")
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id && (q.NhanVien.HOTEN.Contains(searchString)) && (q.NGAYTINHLUONG >= fd && q.NGAYTINHLUONG <= td)).OrderBy(q => q.MAQLL).ToList();
                    }
                    if (fromdate != "" && todate == "")
                    {
                        DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id && (q.NhanVien.HOTEN.Contains(searchString)) && q.NGAYTINHLUONG >= fd).OrderBy(q => q.MAQLL).ToList();
                    }
                    if (fromdate == "" && todate != "")
                    {
                        DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id && (q.NhanVien.HOTEN.Contains(searchString)) && q.NGAYTINHLUONG >= td).OrderBy(q => q.MAQLL).ToList();
                    }
                    if (fromdate == "" && todate == "")
                    {
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id && q.NhanVien.HOTEN.Contains(searchString)).OrderBy(q => q.MAQLL).ToList();
                    }
                }
                else
                    quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.MANV == id).OrderBy(q => q.MAQLL).ToList();
            }
            if (String.IsNullOrEmpty(id) == true)
            {
                if (String.IsNullOrEmpty(searchString) == false || searchString == "")
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.NhanVien.HOTEN.Contains(searchString) && (q.NGAYTINHLUONG >= fd && q.NGAYTINHLUONG <= td)).OrderBy(q => q.MAQLL).ToList();
                    }
                    if (fromdate != "" && todate == "")
                    {
                        DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.NhanVien.HOTEN.Contains(searchString) && q.NGAYTINHLUONG >= fd).OrderBy(q => q.MAQLL).ToList();
                    }
                    if (fromdate == "" && todate != "")
                    {
                        DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.NhanVien.HOTEN.Contains(searchString) && q.NGAYTINHLUONG >= td).OrderBy(q => q.MAQLL).ToList();
                    }
                    if (fromdate == "" && todate == "")
                    {
                        quanLyLuongs = db.QuanLyLuongs.Include(q => q.NhanVien).Where(q => q.NhanVien.HOTEN.Contains(searchString)).OrderBy(q => q.MAQLL).ToList();
                    }
                }
            }

            foreach (var qll in quanLyLuongs)
            {
                dt.Rows.Add(qll.MAQLL, qll.MANV, qll.NhanVien.HOTEN, qll.LUONGCOBAN, qll.THUCLANH, qll.NGAYTINHLUONG?.ToString("dd/MM/yyyy"));
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachLuong.xlsx");
                }
            }
        }
    }
}
