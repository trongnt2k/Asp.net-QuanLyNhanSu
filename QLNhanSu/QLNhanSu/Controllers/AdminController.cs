using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLNhanSu.Models;
using System.Data;
using System.Data.Entity;

namespace QLNhanSu.Controllers
{
    public class AdminController : Controller
    {
        private QLNSEntities db = new QLNSEntities();
        // GET: Admin
        public ActionResult Index()
        {
            var nhanViens = db.NhanViens.Count();
            var phongBans = db.PhongBans.Count();
            var luongs = db.Luongs.Count();
            ViewBag.nhanViens = nhanViens;
            ViewBag.phongBans = phongBans;
            ViewBag.luongs = luongs;
            return View();
        }
    }
}