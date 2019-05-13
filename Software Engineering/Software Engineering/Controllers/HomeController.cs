using System;
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
            ArrayList xValue2 = new ArrayList();
            ArrayList yValue = new ArrayList();
            ArrayList yValue2 = new ArrayList();
            var results = (from c in context.Cars select c);
            results.ToList().ForEach(rs => xValue2.Add(rs.Make));
            foreach (string item in xValue2)
            {
                yValue2.Add(results.Where(s => s.Make == item).Count());
            }
            for(int i=0; i<xValue2.Count;i++)
            {
                if (!xValue.Contains(xValue2[i]))
                {
                    xValue.Add(xValue2[i]);
                    yValue.Add(yValue2[i]);
                }
            }
            new Chart(width: 600, height: 600, theme: ChartTheme.Green)
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