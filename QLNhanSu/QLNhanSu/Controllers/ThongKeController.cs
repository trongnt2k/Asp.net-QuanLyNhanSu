using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using QLNhanSu.Models;
using System.Dynamic;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;

namespace QLNhanSu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ThongKeController : Controller
    {  
        private QLNSEntities db = new QLNSEntities();
        // GET: ThongKe

        public ActionResult Index(string fromdate, string todate)
        {
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            var nhanVien = db.NhanViens.OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { month = s.NGAYBATDAU.Value.Month, year = s.NGAYBATDAU.Value.Year })
                                            .Select(x => new ThongKeNhanVien { count = x.Count(), month = x.Key.month, year = x.Key.year }).ToList();
        if(fromdate != null)
        {
            if (fromdate != "" && todate != "")
            {
                DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                nhanVien = db.NhanViens.Where(n => n.NGAYBATDAU >= fd && n.NGAYBATDAU <= td).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { month = s.NGAYBATDAU.Value.Month, year = s.NGAYBATDAU.Value.Year })
                                            .Select(x => new ThongKeNhanVien { count = x.Count(), month = x.Key.month, year = x.Key.year }).ToList();
            }
            if (fromdate != "" && todate == "")
            {
                DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                nhanVien = db.NhanViens.Where(n => n.NGAYBATDAU >= fd).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { month = s.NGAYBATDAU.Value.Month, year = s.NGAYBATDAU.Value.Year })
                                            .Select(x => new ThongKeNhanVien { count = x.Count(), month = x.Key.month, year = x.Key.year }).ToList();
            }
            if (fromdate == "" && todate != "")
            {
                DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                nhanVien = db.NhanViens.Where(n => n.NGAYBATDAU <= td).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { month = s.NGAYBATDAU.Value.Month, year = s.NGAYBATDAU.Value.Year })
                                            .Select(x => new ThongKeNhanVien { count = x.Count(), month = x.Key.month, year = x.Key.year }).ToList();
            }
        }
            ViewBag.nhanVien = nhanVien;
            return View();
        }
        public ActionResult NhanVienYearStats(string fromdate, string todate)
        {
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            var nhanVien = db.NhanViens.OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { year = s.NGAYBATDAU.Value.Year })
                                            .Select(x => new ThongKeNhanVien { count = x.Count(), year = x.Key.year }).ToList();
            if (fromdate != null)
            {
                if (fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanVien = db.NhanViens.Where(n => n.NGAYBATDAU >= fd && n.NGAYBATDAU <= td).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), year = x.Key.year }).ToList();
                }
                if (fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanVien = db.NhanViens.Where(n => n.NGAYBATDAU >= fd).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), year = x.Key.year }).ToList();
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanVien = db.NhanViens.Where(n => n.NGAYBATDAU <= td).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), year = x.Key.year }).ToList();
                }
            }
            ViewBag.nhanVien = nhanVien;
            return View();
        }
        public ActionResult LuongMonthStats(string fromdate, string todate)
        {
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            var luong = db.QuanLyLuongs.OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { month = s.NGAYTINHLUONG.Value.Month, year = s.NGAYTINHLUONG.Value.Year })
                                            .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), month = x.Key.month, year = x.Key.year }).ToList();
            if (fromdate != null)
            {
                if (fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG >= fd && n.NGAYTINHLUONG <= td).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { month = s.NGAYTINHLUONG.Value.Month, year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), month = x.Key.month, year = x.Key.year }).ToList();
                }
                if (fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG >= fd).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { month = s.NGAYTINHLUONG.Value.Month, year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), month = x.Key.month, year = x.Key.year }).ToList();
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG <= td).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { month = s.NGAYTINHLUONG.Value.Month, year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), month = x.Key.month, year = x.Key.year }).ToList();
                }
            }
            ViewBag.luong = luong;
            return View();
        }
        public ActionResult LuongYearStats(string fromdate, string todate)
        {
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            var luong = db.QuanLyLuongs.OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { year = s.NGAYTINHLUONG.Value.Year })
                                            .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), year = x.Key.year }).ToList();
            if (fromdate != null)
            {
                if (fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG >= fd && n.NGAYTINHLUONG <= td).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), year = x.Key.year }).ToList();
                }
                if (fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG >= fd).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), year = x.Key.year }).ToList();
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG <= td).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), year = x.Key.year }).ToList();
                }
            }
            ViewBag.luong = luong;
            return View();
        }
        [HttpPost]
        public FileResult NhanVienMonthExport(string fromdate, string todate)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Tháng"),
                new DataColumn("Số nhân viên"),
                });
            var nhanViens = db.NhanViens.OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { month = s.NGAYBATDAU.Value.Month, year = s.NGAYBATDAU.Value.Year })
                                            .Select(x => new ThongKeNhanVien { count = x.Count(), month = x.Key.month, year = x.Key.year }).ToList();
            if (fromdate != null)
            {
                if (fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.NGAYBATDAU >= fd && n.NGAYBATDAU <= td).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { month = s.NGAYBATDAU.Value.Month, year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), month = x.Key.month, year = x.Key.year }).ToList();
                }
                if (fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.NGAYBATDAU >= fd).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { month = s.NGAYBATDAU.Value.Month, year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), month = x.Key.month, year = x.Key.year }).ToList();
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.NGAYBATDAU <= td).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { month = s.NGAYBATDAU.Value.Month, year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), month = x.Key.month, year = x.Key.year }).ToList();
                }
            }

            foreach (var nv in nhanViens)
            {
                dt.Rows.Add(nv.month+"/"+nv.year, nv.count);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeNhanVienThang.xlsx");
                }
            }
        }
        public FileResult NhanVienYearExport(string fromdate, string todate)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Năm"),
                new DataColumn("Số nhân viên"),
                });
            var nhanViens = db.NhanViens.OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { year = s.NGAYBATDAU.Value.Year })
                                            .Select(x => new ThongKeNhanVien { count = x.Count(), year = x.Key.year }).ToList();
            if (fromdate != null)
            {
                if (fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.NGAYBATDAU >= fd && n.NGAYBATDAU <= td).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), year = x.Key.year }).ToList();
                }
                if (fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.NGAYBATDAU >= fd).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), year = x.Key.year }).ToList();
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    nhanViens = db.NhanViens.Where(n => n.NGAYBATDAU <= td).OrderBy(n => n.NGAYBATDAU).GroupBy(s => new { year = s.NGAYBATDAU.Value.Year })
                                                .Select(x => new ThongKeNhanVien { count = x.Count(), year = x.Key.year }).ToList();
                }
            }

            foreach (var nv in nhanViens)
            {
                dt.Rows.Add(nv.year, nv.count);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeNhanVienNam.xlsx");
                }
            }
        }
        public FileResult LuongMonthExport(string fromdate, string todate)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Tháng"),
                new DataColumn("Tổng tiền lương"),
                });
            var luong = db.QuanLyLuongs.OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { month = s.NGAYTINHLUONG.Value.Month, year = s.NGAYTINHLUONG.Value.Year })
                                            .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), month = x.Key.month, year = x.Key.year }).ToList();
            if (fromdate != null)
            {
                if (fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG >= fd && n.NGAYTINHLUONG <= td).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { month = s.NGAYTINHLUONG.Value.Month, year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), month = x.Key.month, year = x.Key.year }).ToList();
                }
                if (fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG >= fd).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { month = s.NGAYTINHLUONG.Value.Month, year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), month = x.Key.month, year = x.Key.year }).ToList();
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG <= td).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { month = s.NGAYTINHLUONG.Value.Month, year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), month = x.Key.month, year = x.Key.year }).ToList();
                }
            }

            foreach (var l in luong)
            {
                dt.Rows.Add(l.month + "/" + l.year, l.sum);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeLuongThang.xlsx");
                }
            }
        }
        public FileResult LuongYearExport(string fromdate, string todate)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Năm"),
                new DataColumn("Tổng tiền lương"),
                });
            var luong = db.QuanLyLuongs.OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { year = s.NGAYTINHLUONG.Value.Year })
                                            .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), year = x.Key.year }).ToList();
            if (fromdate != null)
            {
                if (fromdate != "" && todate != "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG >= fd && n.NGAYTINHLUONG <= td).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), year = x.Key.year }).ToList();
                }
                if (fromdate != "" && todate == "")
                {
                    DateTime fd = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG >= fd).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), year = x.Key.year }).ToList();
                }
                if (fromdate == "" && todate != "")
                {
                    DateTime td = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    luong = db.QuanLyLuongs.Where(n => n.NGAYTINHLUONG <= td).OrderBy(n => n.NGAYTINHLUONG).GroupBy(s => new { year = s.NGAYTINHLUONG.Value.Year })
                                                .Select(x => new ThongKeLuong { sum = (float)x.Sum(s => s.THUCLANH), year = x.Key.year }).ToList();
                }
            }

            foreach (var l in luong)
            {
                dt.Rows.Add(l.year, l.sum);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeLuongNam.xlsx");
                }
            }
        }
    }
}