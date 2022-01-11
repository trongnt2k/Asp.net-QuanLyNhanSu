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
    public class PhongBanController : Controller
    {
        private QLNSEntities db = new QLNSEntities();

        // GET: PhongBan
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var phongBans = db.PhongBans.OrderBy(p => p.MAPB);
            return View(phongBans.ToPagedList(pageNumber, pageSize));

        }

        // GET: PhongBan/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.PBName = db.PhongBans.Find(id).TENPB;
            //PhongBan phongBan = db.PhongBans.Find(id);
            //if (phongBan == null)
            //{
            //    return HttpNotFound();
            //}
            var nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Where(n => n.MAPB == id).OrderBy(n => n.MANV);
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
    }
}
