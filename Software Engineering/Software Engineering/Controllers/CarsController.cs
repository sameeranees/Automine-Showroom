using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Software_Engineering.Models;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Software_Engineering.Controllers
{
    public class CarsController : Controller
    {
        Software_EngineeringEntities1 db = new Software_EngineeringEntities1();
        // GET: Cars
        public ActionResult Index()
        {
            var car = db.Cars.Include(q => q.Customer);
            car = db.Cars.Include(q => q.Tracker);
            car=db.Cars.Include(q => q.Insurance);
            return View(db.Cars.ToList());
        }
        public ActionResult Sell()
        {
            var car = db.Cars.Include(q => q.Customer);
            car = db.Cars.Include(q => q.Tracker);
            car = db.Cars.Include(q => q.Insurance);
            var viewModel = new ViewModel
            {
                Sold = db.Cars.Where(x => x.soldDate != null).ToList(),
                UnSold = db.Cars.Where(x => x.soldDate == null).ToList()
            };
            return View(viewModel);
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.customerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.trackerId= new SelectList(db.Trackers,"Id","Company");
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company");
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Model,Make,Mileage,Year,CC,buyingPrice,sellingPrice,maintainanceCost,Condition,Imported,ownerName,purchasedDate,soldDate,registerationNo,customerId,trackerId,InsuranceId")] Car car)
        {
            if (ModelState.IsValid)
            {
                //cagtegory.SecretCode = GenerateSecretCode();
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerId = new SelectList(db.Customers, "Id", "Name", car.customerId);
            ViewBag.trackerId = new SelectList(db.Trackers, "Id", "Company",car.trackerId);
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company",car.insuranceId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerId = new SelectList(db.Customers, "Id", "Name", car.customerId);
            ViewBag.trackerId = new SelectList(db.Trackers, "Id", "Company", car.trackerId);
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company", car.insuranceId);
            return View(car);
        }
        public ActionResult SellCar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerId = new SelectList(db.Customers, "Id", "Name", car.customerId);
            ViewBag.trackerId = new SelectList(db.Trackers, "Id", "Company", car.trackerId);
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company", car.insuranceId);
            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Model,Make,Mileage,Year,CC,buyingPrice,sellingPrice,maintainanceCost,Condition,Imported,ownerName,purchasedDate,soldDate,registerationNo,customerId,trackerId,InsuranceId")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerId = new SelectList(db.Customers, "Id", "Name", car.customerId);
            ViewBag.trackerId = new SelectList(db.Trackers, "Id", "Company", car.trackerId);
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company", car.insuranceId);
            return View(car);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellCar([Bind(Include = "Id,Model,Make,Mileage,Year,CC,buyingPrice,sellingPrice,maintainanceCost,Condition,Imported,ownerName,purchasedDate,soldDate,registerationNo,customerId,trackerId,InsuranceId")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                return RedirectToAction("Index");
            }
            ViewBag.customerId = new SelectList(db.Customers, "Id", "Name", car.customerId);
            ViewBag.trackerId = new SelectList(db.Trackers, "Id", "Company", car.trackerId);
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company", car.insuranceId);
            return View(car);
        }
        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
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
