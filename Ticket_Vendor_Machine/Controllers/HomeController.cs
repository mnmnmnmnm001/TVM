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
        public JsonResult CalculateFare(int destinationId, int quantity = 1)
        {
            if (quantity < 1) quantity = 1;
            if (quantity > 9) quantity = 9;

            decimal unitFare = _fareCalc.GetUnitFare(destinationId);
            decimal totalFare = _fareCalc.GetTotalFare(destinationId, quantity);

            return Json(new { unitFare, totalFare }, JsonRequestBehavior.AllowGet);
        }

    }
}