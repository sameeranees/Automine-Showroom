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
    public class TrackerController : Controller
    {
        // GET: Tracker
        Software_EngineeringEntities1 db = new Software_EngineeringEntities1();
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Index()
        {
            return View(db.Trackers.ToList());
        }

        // GET: Tracker/Details/5
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tracker tracker = db.Trackers.Find(id);
            if (tracker == null)
            {
                return HttpNotFound();
            }

            return View(tracker);
        }

        // GET: Tracker/Create
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tracker/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Create([Bind(Include = "Company,Package")] Tracker tracker)
        {
            if (ModelState.IsValid)
            {
                //cagtegory.SecretCode = GenerateSecretCode();
                db.Trackers.Add(tracker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tracker);
        }

        // GET: Tracker/Edit/5
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracker tracker = db.Trackers.Find(id);
            if (tracker == null)
            {
                return HttpNotFound();
            }
            return View(tracker);
        }

        // POST: Tracker/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Edit([Bind(Include = "Company,Package")] Tracker tracker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tracker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tracker);
        }

        // GET: Tracker/Delete/5
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracker tracker = db.Trackers.Find(id);
            if (tracker == null)
            {
                return HttpNotFound();
            }
            return View(tracker);
        }

        // POST: Tracker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Delete(int id)
        {
            Tracker tracker = db.Trackers.Find(id);
            db.Trackers.Remove(tracker);
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
