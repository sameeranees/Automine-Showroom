﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Software_Engineering.Models;
using System.Collections;
using System.Web.Helpers;

namespace Software_Engineering.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles ="Admin,Finance Manager, Manager")]
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult CharterColumn()
        {
            var context = new Software_EngineeringEntities1();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var results = (from c in context.Cars select c);
            results.ToList().ForEach(rs => xValue.Add(rs.Make));
            results.ToList().ForEach(rs => yValue.Add(rs.Make.Count()));

            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
            .AddTitle("Cars")
            .AddSeries("Default", chartType: "Pie", xValue: xValue, yValues: yValue)
            .Write();
            return null;
        }
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "Admin,Finance Manager, Manager")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}