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
        public bool isModified1 = false;
        public bool isModified2 = false;
        public bool isModified3 = false;
        public bool isModified4 = false;
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Index()
        {
            var car = db.Cars.Include(q => q.Customer);
            car = db.Cars.Include(q => q.Tracker);
            car=db.Cars.Include(q => q.Insurance);
            return View(db.Cars.ToList());
        }

        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Renew_List()
        {
            DateTime now = DateTime.Now;
            DateTime nowPlus30Days = now.AddDays(30);
            var car = db.Cars.Include(q => q.Customer);
            car = db.Cars.Include(q => q.Tracker);
            car = db.Cars.Include(q => q.Insurance);
            var viewModel = new ViewModel
            {
                Sold = db.Cars.Where(q => q.insExpiry.Value <= nowPlus30Days).OrderBy(q=>q.insExpiry.Value).ToList(),
                UnSold = db.Cars.Where(q => q.traExpiry.Value <= nowPlus30Days).OrderBy(q => q.traExpiry.Value).ToList()
            };
            return View(viewModel);
        }

        public int TotalExpiries()
        {
            DateTime now = DateTime.Now;
            DateTime nowPlus30Days = now.AddDays(30);
            var car = db.Cars.Include(q => q.Customer);
            car = db.Cars.Include(q => q.Tracker);
            car = db.Cars.Include(q => q.Insurance);
            var total = db.Cars.Where(q => q.insExpiry.Value <= nowPlus30Days).OrderBy(q => q.insExpiry.Value).ToList().Count + db.Cars.Where(q => q.traExpiry.Value <= nowPlus30Days).OrderBy(q => q.traExpiry.Value).ToList().Count;
            return total;
        }
        //[Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Cars()
        {
            var car = db.Cars.Include(q => q.Customer);
            car = db.Cars.Include(q => q.Tracker);
            car = db.Cars.Include(q => q.Insurance);
            car=db.Cars.Where(q=>q.soldDate==null).OrderBy(q=>q.Make);
            return View(car.ToList());
        }
        //[Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Home()
        {
            return View();
        }
        [Authorize(Roles = "Finance Manager")]
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Create()
        {
            ViewBag.customerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.trackerId= new SelectList(db.Trackers,"Id","Company");
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company");
            return View();
        }

        public Closing Update(Closing closing, long diff,bool newl)
        {
            //bool updated = false;
            var query2 = from m in db.Closings
                         where m.Month < closing.Month && m.Year==closing.Year || m.Year < closing.Year
                         orderby m.Month, m.Year
                         select m;
            if (newl == true)
            {
                foreach (var item in query2.ToList())
                {
                    if ((item.Month == closing.Month - 1 && item.Year == closing.Year) || (item.Month == 12 && item.Year == closing.Year - 1))
                    {
                        closing.ClosingBalance += item.ClosingBalance;
                        //updated = true;
                        break;
                    }
                }
            }
            query2 = from m in db.Closings
                     where (m.Month > closing.Month && m.Year==closing.Year) || m.Year > closing.Year
                     orderby m.Month, m.Year
                     select m;
            foreach (var item in query2.ToList())
            {
                if ((item.Month > closing.Month && item.Year == closing.Year) || item.Year > closing.Year)
                {
                    item.ClosingBalance += diff;
                    db.Entry(item).State = EntityState.Modified;
                }
            }
            return closing;
        }

        public Closing CUpdate(Car car, Closing closing,bool newl)
        {
            var buyprice = car.buyingPrice;
            long sellprice = 0;
            long mcost = 0;
            if (car.soldDate != null && car.soldDate.Value.Month == closing.Month && car.soldDate.Value.Year == closing.Year)
            {
                sellprice = car.sellingPrice.Value;
                mcost = car.maintainanceCost.Value;
                closing.ClosingBalance += car.sellingPrice - car.maintainanceCost;
            }
            else if (car.soldDate != null)
            {
                NewSell(car);
            }
            var diff = sellprice - buyprice - mcost;
            //bool updated = false;
            closing = Update(closing, diff,newl);
            return closing;
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Create([Bind(Exclude = "image1,image2,image3,image4")]Car car,HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3, HttpPostedFileBase image4)
        {
            
            if (ModelState.IsValid)
            {
                if (car.trackerId != null && car.soldDate!=null)
                {
                    car.traExpiry = car.soldDate;
                    car.traExpiry.Value.AddYears(1);
                }
                if (car.insuranceId != null && car.soldDate != null)
                {
                    car.insExpiry = car.soldDate;
                    car.insExpiry.Value.AddYears(1);
                }
                var query = from m in db.Closings where m.Month == car.purchasedDate.Month && m.Year == car.purchasedDate.Year select m;
                var data = query.ToList();
                Closing closing = new Closing();
                if (data.Count == 0)
                {
                    closing.Month = car.purchasedDate.Month;
                    closing.Year = car.purchasedDate.Year;
                    closing.ClosingBalance = 0 - car.buyingPrice;
                    closing = CUpdate(car, closing, true);
                    db.Closings.Add(closing);
                }
                else
                {
                    closing = data[0];
                    closing.ClosingBalance -= car.buyingPrice;
                    closing = CUpdate(car, closing, false);
                    db.Entry(closing).State = EntityState.Modified;
                }
                if (image1 != null)
                {
                    car.Image = new byte[image1.ContentLength];
                    image1.InputStream.Read(car.Image, 0, image1.ContentLength);
                }
                if (image2 != null)
                {
                    car.Image2 = new byte[image2.ContentLength];
                    image2.InputStream.Read(car.Image2, 0, image2.ContentLength);
                }
                if (image3 != null)
                {
                    car.Image3 = new byte[image3.ContentLength];
                    image3.InputStream.Read(car.Image3, 0, image3.ContentLength);
                }
                if (image4 != null)
                {
                    car.Image4 = new byte[image4.ContentLength];
                    image4.InputStream.Read(car.Image4, 0, image4.ContentLength);
                }
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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


        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult RenewInsurance(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            Customer customer = db.Customers.Find(car.customerId);
            if (car == null || customer==null)
            {
                return HttpNotFound();
            }
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company", car.insuranceId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult RenewInsurance(Car car)
        {
            if (ModelState.IsValid)
            {
                var carl = db.Cars.Find(car.Id);
                carl.insExpiry = car.insExpiry;
                db.Entry(carl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Renew_List");
            }
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company", car.insuranceId);
            return View(car);
        }

        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult RenewTracker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            Customer customer = db.Customers.Find(car.customerId);
            if (car == null || customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.trackerId = new SelectList(db.Trackers, "Id", "Company", car.trackerId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult RenewTracker(Car car)
        {
            if (ModelState.IsValid)
            {
                var carl = db.Cars.Find(car.Id);
                carl.traExpiry = car.traExpiry;
                db.Entry(carl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Renew_List");
            }
            ViewBag.trackerId = new SelectList(db.Trackers, "Id", "Company", car.trackerId);
            return View(car);
        }

        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
        
        public Closing NewSell(Car car)
        {
            var closing = new Closing();
            closing.Month = car.soldDate.Value.Month;
            closing.Year = car.soldDate.Value.Year;
            var query = from m in db.Closings
                        where m.Month == closing.Month && m.Year == closing.Year
                        select m;
            var result = query.ToList();
            long diff = car.sellingPrice.Value - car.maintainanceCost.Value;
            if (result.Count != 0)
            {
                closing = result[0];
                closing.ClosingBalance += car.sellingPrice-car.maintainanceCost;
                closing = Update(closing, diff, false);
                db.Entry(closing).State = EntityState.Modified;
            }
            else
            {
                closing.ClosingBalance = 0 + car.sellingPrice-car.maintainanceCost;
                closing = Update(closing, diff, true);
                db.Closings.Add(closing);
            }
            return closing;
        }

        public Closing NewPurchase(Car car)
        {
            var closing = new Closing();
            closing.Month = car.purchasedDate.Month;
            closing.Year = car.purchasedDate.Year;
            var query = from m in db.Closings
                        where m.Month == closing.Month && m.Year == closing.Year
                        select m;
            var result = query.ToList();
            long diff = -car.buyingPrice;
            if (result.Count != 0)
            {
                closing = result[0];
                closing.ClosingBalance -= car.buyingPrice;
                closing = Update(closing, diff, false);
                db.Entry(closing).State = EntityState.Modified;
            }
            else
            {
                closing.ClosingBalance = 0 - car.buyingPrice;
                closing = Update(closing, diff, true);
                db.Closings.Add(closing);
            }
            return closing;
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Edit([Bind(Exclude = "image1,image2,image3,image4,modified1,modified2,modified3,modified4")]Car car,HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3, HttpPostedFileBase image4,bool modified1, bool modified2, bool modified3, bool modified4)
        {
            
            if (ModelState.IsValid)
            {
                if (car.traExpiry == null && car.trackerId != null && car.soldDate != null)
                {
                    car.traExpiry = car.soldDate;
                    car.traExpiry.Value.AddYears(1);
                }
                if (car.insExpiry == null && car.insuranceId != null && car.soldDate != null)
                {
                    car.insExpiry = car.soldDate;
                    car.insExpiry.Value.AddYears(1);
                }
                var carl = db.Cars.Find(car.Id);
                var sDate = db.Cars.Find(car.Id).soldDate;
                var pDate = db.Cars.Find(car.Id).purchasedDate;
                if (car.soldDate != sDate || car.sellingPrice != carl.sellingPrice || car.maintainanceCost != carl.maintainanceCost)
                {
                    var query = from m in db.Closings
                                where m.Month == sDate.Value.Month && m.Year == sDate.Value.Year
                                select m;
                    if (query.ToList().Count > 0)
                    {
                        var result = query.ToList()[0];
                        result.ClosingBalance -= carl.sellingPrice;
                        result.ClosingBalance += carl.maintainanceCost;
                        result=Update(result, carl.maintainanceCost.Value - carl.sellingPrice.Value, false);
                        db.Entry(result).State = EntityState.Modified;
                    }
                    if (car.soldDate != null)
                    {
                        NewSell(car);
                    }
                }
                if (car.purchasedDate != pDate || car.buyingPrice != carl.buyingPrice)
                {
                    var query = from m in db.Closings
                                where m.Month == pDate.Month && m.Year == pDate.Year
                                select m;
                    if (query.ToList().Count > 0)
                    {
                        var result = query.ToList()[0];
                        result.ClosingBalance += carl.buyingPrice;
                        result=Update(result, carl.buyingPrice, false);
                        db.Entry(result).State = EntityState.Modified;
                    }
                    NewPurchase(car);
                }
                if (image1 != null)
                {
                    car.Image = new byte[image1.ContentLength];
                    image1.InputStream.Read(car.Image, 0, image1.ContentLength);
                }
                if (image2 != null)
                {
                    car.Image2 = new byte[image2.ContentLength];
                    image2.InputStream.Read(car.Image2, 0, image2.ContentLength);
                }
                if (image3 != null)
                {
                    car.Image3 = new byte[image3.ContentLength];
                    image3.InputStream.Read(car.Image3, 0, image3.ContentLength);
                }
                if (image4 != null)
                {
                    car.Image4 = new byte[image4.ContentLength];
                    image4.InputStream.Read(car.Image4, 0, image4.ContentLength);
                }
                if (image1 == null)
                {
                    car.Image = db.Cars.Where(i => i.Id == car.Id).Select(i => i.Image).ToList()[0];
                }
                if (image2 == null)
                {
                    car.Image2 = db.Cars.Where(i => i.Id == car.Id).Select(i => i.Image2).ToList()[0];
                }
                if (image3 == null)
                {
                    car.Image3 = db.Cars.Where(i => i.Id == car.Id).Select(i => i.Image3).ToList()[0];
                }
                if (image4 == null)
                {
                    car.Image4 = db.Cars.Where(i => i.Id == car.Id).Select(i => i.Image4).ToList()[0];
                }
                if (modified1 == true)
                {
                    car.Image = null;
                }
                if (modified2 == true)
                {
                    car.Image2 = null;
                }
                if (modified3 == true)
                {
                    car.Image3 = null;
                }
                if (modified4 == true)
                {
                    car.Image4 = null;
                }
                var card = db.Cars.Find(car.Id);
                db.Entry(card).State = EntityState.Detached;
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult SellCar([Bind(Include = "Id,Model,Make,Mileage,Year,CC,buyingPrice,sellingPrice,maintainanceCost,Condition,Imported,ownerName,purchasedDate,soldDate,registerationNo,customerId,trackerId,InsuranceId")] Car car)
        {
            if (ModelState.IsValid)
            {
                if (car.traExpiry == null && car.trackerId != null && car.soldDate != null)
                {
                    car.traExpiry = car.soldDate;
                    car.traExpiry.Value.AddYears(1);
                }
                if (car.insExpiry == null && car.insuranceId != null && car.soldDate != null)
                {
                    car.insExpiry = car.soldDate;
                    car.insExpiry.Value.AddYears(1);
                }
                var carl = db.Cars.Find(car.Id);
                var sDate = db.Cars.Find(car.Id).soldDate;
                if (car.soldDate != sDate || car.sellingPrice != carl.sellingPrice || car.maintainanceCost != carl.maintainanceCost)
                {
                    var query = from m in db.Closings
                                where m.Month == sDate.Value.Month && m.Year == sDate.Value.Year
                                select m;
                    if (query.ToList().Count > 0)
                    {
                        var result = query.ToList()[0];
                        result.ClosingBalance -= carl.sellingPrice;
                        result.ClosingBalance += carl.maintainanceCost;
                        result=Update(result, carl.maintainanceCost.Value - carl.sellingPrice.Value, false);
                        db.Entry(result).State = EntityState.Modified;
                    }
                    if (car.soldDate != null)
                    {
                        NewSell(car);
                    }
                }
                var card = db.Cars.Find(car.Id);
                db.Entry(card).State = EntityState.Detached;
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Sell");
            }
            ViewBag.customerId = new SelectList(db.Customers, "Id", "Name", car.customerId);
            ViewBag.trackerId = new SelectList(db.Trackers, "Id", "Company", car.trackerId);
            ViewBag.insuranceId = new SelectList(db.Insurances, "Id", "Company", car.insuranceId);
            return View(car);
        }
        // GET: Cars/Delete/5
        [Authorize(Roles ="Admin,Finance Manager, Manager")]
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
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
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
