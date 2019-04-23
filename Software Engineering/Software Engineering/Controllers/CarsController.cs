using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Software_Engineering.Models;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Microsoft.Reporting.WebForms;

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
        public ActionResult Report(int? id)
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
            LocalReport localreport= new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Report/Rdlc/Cars.rdlc");

            int custid = Convert.ToInt32(db.Cars.Where(x => x.Id == id).Select(x => x.customerId).First());
            int insuid = Convert.ToInt32(db.Cars.Where(x => x.Id == id).Select(x => x.insuranceId).First());
            int tracid = Convert.ToInt32(db.Cars.Where(x => x.Id == id).Select(x => x.trackerId).First());

            ReportDataSource reportdatasource = new ReportDataSource();
            reportdatasource.Name = "DataSet1";
            reportdatasource.Value = db.Cars.Where(x=> x.Id==id).ToList();
            localreport.DataSources.Add(reportdatasource);

            ReportDataSource reportdatasource2 = new ReportDataSource();
            reportdatasource2.Name = "DataSet2";
            reportdatasource2.Value = db.Customers.Where(x => x.Id == custid).ToList();
            localreport.DataSources.Add(reportdatasource2);

            ReportDataSource reportdatasource3 = new ReportDataSource();
            reportdatasource3.Name = "DataSet3";
            reportdatasource3.Value = db.Insurances.Where(x => x.Id == insuid).ToList();
            localreport.DataSources.Add(reportdatasource3);

            ReportDataSource reportdatasource4 = new ReportDataSource();
            reportdatasource4.Name = "DataSet4";
            reportdatasource4.Value = db.Trackers.Where(x => x.Id == tracid).ToList();
            localreport.DataSources.Add(reportdatasource4);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension = "pdf";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment; filename=Urls." + fileNameExtension);
            return File(renderedBytes, fileNameExtension);
            return View(car);
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
