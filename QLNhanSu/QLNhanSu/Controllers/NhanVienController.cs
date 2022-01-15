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
using ClosedXML.Excel;
using System.Globalization;

namespace QLNhanSu.Controllers
{
    public class NhanVienController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: NhanVien
        public ActionResult Index(int? page, string searchString, string fromdate, string todate)
        {
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.searchString = searchString;
            var nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).OrderBy(n => n.MANV);
            if(searchString != null)
            {
                if(fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.HOTEN.Contains(searchString) && (n.NGAYBATDAU >= fd && n.NGAYBATDAU <= td)).OrderBy(n => n.MANV);
                }
                if(fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.HOTEN.Contains(searchString) && n.NGAYBATDAU >= fd).OrderBy(n => n.MANV);
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.HOTEN.Contains(searchString) && n.NGAYBATDAU >= td).OrderBy(n => n.MANV);
                }
                if (fromdate == "" && todate == "")
                {
                    nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.HOTEN.Contains(searchString)).OrderBy(n => n.MANV);
                }
            }
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
        [HttpPost]
        public FileResult Export(string searchString, string fromdate, string todate)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[12] {
                new DataColumn("Mã nhân viên"),
                new DataColumn("Họ tên"),
                new DataColumn("Dân tộc"),
                new DataColumn("Giới tính"),
                new DataColumn("Số điện thoại"),
                new DataColumn("Quê quán"),
                new DataColumn("Ngày sinh"),
                new DataColumn("Ngày bắt đầu"),
                new DataColumn("Chuyên ngành"),
                new DataColumn("Chức vụ"),
                new DataColumn("Phòng ban"),
                new DataColumn("Trình độ học vấn")
                });
            var nhanViens = db.NhanViens.OrderBy(n => n.MANV).ToList();
            if (searchString != null)
            {
                if (fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.HOTEN.Contains(searchString) && (n.NGAYBATDAU >= fd && n.NGAYBATDAU <= td)).OrderBy(n => n.MANV).ToList();
                }
                if (fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.HOTEN.Contains(searchString) && n.NGAYBATDAU >= fd).OrderBy(n => n.MANV).ToList();
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.HOTEN.Contains(searchString) && n.NGAYBATDAU >= td).OrderBy(n => n.MANV).ToList();
                }
            }

            foreach (var nv in nhanViens)
            {
                var chucvu = "";
                var phongban = "";
                var trinhdohocvan = "";
                if (nv.PhongBan != null)
                {
                    PhongBan pb = db.PhongBans.Find(nv.MAPB);
                    phongban = pb.TENPB;
                }
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
                dt.Rows.Add(nv.MANV, nv.HOTEN, nv.DANTOC, nv.GIOITINH, nv.SODIENTHOAI, nv.QUEQUAN, nv.NGAYSINH?.ToString("dd/MM/yyyy"), nv.NGAYBATDAU?.ToString("dd/MM/yyyy"), nv.CHUYENNGANH, chucvu, phongban, trinhdohocvan);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NhanVien.xlsx");
                }
            }
        }
    }
}
