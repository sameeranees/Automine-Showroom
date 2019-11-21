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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Index()
        {
            return View(db.Insurances.ToList());
        }

        // GET: Insurance/Details/5
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insurance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Edit([Bind(Include = "Id,Company,Package")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                var card = db.Insurances.Find(insurance.Id);
                db.Entry(card).State = EntityState.Detached;
                insurance.Cars = card.Cars;
                db.Entry(insurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insurance);
        }

        // GET: Insurance/Delete/5
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
