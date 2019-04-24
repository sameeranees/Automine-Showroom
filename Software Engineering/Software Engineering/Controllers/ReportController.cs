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
using System.Diagnostics;

namespace Software_Engineering.Controllers
{
    public class ReportController : Controller
    {
        Software_EngineeringEntities1 db = new Software_EngineeringEntities1();
        // GET: Report
        [Authorize(Roles = "Finance Manager")]
        public ActionResult Report()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Finance Manager")]
        public ActionResult Report([Bind(Include ="Month")] Car cars)
        {
            int B=0;
            if (cars.Month == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cars.Month = cars.Month.ToUpper();
            switch (cars.Month){
                case "JANUARY":
                    B = 1;
                    break;
                case "FEBRUARY":
                    B = 2;
                    break;
                case "MARCH":
                    B = 3;
                    break;
                case "APRIL":
                    B = 4;
                    break;
                case "MAY":
                    B = 5;
                    break;
                case "JUNE":
                    B = 6;
                    break;
                case "JULY":
                    B = 7;
                    break;
                case "AUGUST":
                    B = 8;
                    break;
                case "SEPTEMBER":
                    B = 9;
                    break;
                case "OCTOBER":
                    B = 10;
                    break;
                case "NOVEMBER":
                    B = 11;
                    break;
                case "DECEMBER":
                    B = 12;
                    break;
                default:
                    B = 0;
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Report/Rdlc/Total.rdlc");

            ReportDataSource reportdatasource = new ReportDataSource();
            reportdatasource.Name = "DataSet1";
            reportdatasource.Value = db.Cars.Where(q => q.soldDate.Value.Month== B).ToList();
            localreport.DataSources.Add(reportdatasource);

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
            return View(cars);
        }
    }
}