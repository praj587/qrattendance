using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ParveenEvent.Models;

namespace ParveenEvent.Controllers
{
    public class UserController : Controller
    {
        private ParveenEntities db = new ParveenEntities();

        // GET: User
        public ActionResult Index()
        {
            return View(db.tbl_Reg.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Reg tbl_Reg = db.tbl_Reg.Find(id);
            if (tbl_Reg == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Reg);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cus_Id,Cus_Name,Cus_Mobile,Cus_Company,Cus_Occupation,IsPresent,IsGift,Cus_key,Cus_Email")] tbl_Reg tbl_Reg)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Reg.Add(tbl_Reg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Reg);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Reg tbl_Reg = db.tbl_Reg.Find(id);
            if (tbl_Reg == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Reg);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cus_Id,Cus_Name,Cus_Mobile,Cus_Company,Cus_Occupation,IsPresent,IsGift,Cus_key,Cus_Email")] tbl_Reg tbl_Reg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Reg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Reg);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Reg tbl_Reg = db.tbl_Reg.Find(id);
            if (tbl_Reg == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Reg);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Reg tbl_Reg = db.tbl_Reg.Find(id);
            db.tbl_Reg.Remove(tbl_Reg);
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
