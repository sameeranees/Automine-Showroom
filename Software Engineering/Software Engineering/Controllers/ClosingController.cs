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
    public class ClosingController : Controller
    {
        // GET: Closing
        Software_EngineeringEntities1 db = new Software_EngineeringEntities1();

        [Authorize(Roles = "Finance Manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Finance Manager")]
        public ActionResult Create(Closing closing)
        {
            if (ModelState.IsValid)
            {
                var query2 = from m in db.Closings
                             where m.Month > closing.Month || m.Year >= closing.Year
                             orderby m.Month, m.Year
                             select m;
                long diff = closing.ClosingBalance.Value;
                foreach (var item in query2.ToList())
                {
                    if ((item.Month > closing.Month && item.Year == closing.Year) || item.Year > closing.Year)
                    {
                        item.ClosingBalance += diff;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                //cagtegory.SecretCode = GenerateSecretCode();
                db.Closings.Add(closing);
                db.SaveChanges();
                return Redirect("~/Report/Report");
            }
            return View(closing);
        }
    }
}