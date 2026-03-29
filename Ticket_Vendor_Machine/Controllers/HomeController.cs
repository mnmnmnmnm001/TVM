using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket_Vendor_Machine.ServiceFunction;

namespace Ticket_Vendor_Machine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private readonly FareCalculator _fareCalc = new FareCalculator();

        [HttpGet]
        public JsonResult CalculateFare(int destinationId)
        {
            decimal fare = _fareCalc.Calculate(destinationId);
            return Json(new { fare = fare }, JsonRequestBehavior.AllowGet);
        }

    }
}