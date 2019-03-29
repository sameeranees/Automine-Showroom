using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Software_Engineering.Models;
using System.Net;
using System.Data.Entity;

namespace Software_Engineering.Controllers
{
    public class InsuranceController : Controller
    {
        // GET: Insurance
        Software_EngineeringEntities1 db = new Software_EngineeringEntities1();
        public ActionResult Index()
        {
            return View(db.Insurances.ToList());
        }

        // GET: Insurance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Insurance insurance = db.Insurances.Find(id);
            if (insurance == null)
            {
                return HttpNotFound();
            }

            return View(insurance);
        }

        // GET: Tracker/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insurance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Company,Package")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                //cagtegory.SecretCode = GenerateSecretCode();
                db.Insurances.Add(insurance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insurance);
        }

        // GET: Insurance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insurance insurance = db.Insurances.Find(id);
            if (insurance == null)
            {
                return HttpNotFound();
            }
            return View(insurance);
        }

        // POST: Insurance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Company,Package")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insurance);
        }

        // GET: Insurance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insurance insurance = db.Insurances.Find(id);
            if (insurance == null)
            {
                return HttpNotFound();
            }
            return View(insurance);
        }

        // POST: Insurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Insurance insurance = db.Insurances.Find(id);
            db.Insurances.Remove(insurance);
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
