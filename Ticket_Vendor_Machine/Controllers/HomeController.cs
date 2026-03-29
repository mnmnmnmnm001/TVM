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

        public ActionResult Payment(string toStation, int qty, string total, string ticketType = "single", string lang = "vn")
        {
            ViewBag.ToStation = toStation ?? "---";
            ViewBag.Quantity = qty;
            ViewBag.Total = total ?? "0 VNĐ";
            ViewBag.Lang = lang; // Lưu ngôn ngữ vào ViewBag
            ViewBag.TicketType = ticketType ?? "single";

            // Friendly label
            switch (ViewBag.TicketType as string)
            {
                case "1day":
                    ViewBag.TicketTypeLabel = "Vé 1 ngày";
                    // Valid for 24 hours from purchase
                    ViewBag.ValidUntil = DateTime.Now.AddHours(24).ToString("g");
                    break;
                case "3day":
                    ViewBag.TicketTypeLabel = "Vé 3 ngày";
                    break;
                case "single":
                default:
                    ViewBag.TicketTypeLabel = "Vé một chặng";
                    break;
            }

            return View();
        }
        private readonly FareCalculator _fareCalc = new FareCalculator();

        [HttpGet]
        public JsonResult CalculateFare(int destinationId, int quantity = 1, string ticketType = "single")
        {
            if (quantity < 1) quantity = 1;
            if (quantity > 9) quantity = 9;

            decimal unitFare = _fareCalc.GetUnitFare(destinationId);
            decimal totalFare = _fareCalc.GetTotalFare(destinationId, quantity);

            // If user selects 1-day ticket, use fixed daily price per passenger
            if (!string.IsNullOrEmpty(ticketType) && ticketType == "1day")
            {
                unitFare = 30000m; // fixed price per passenger for 1-day ticket
                totalFare = unitFare * quantity;
            }

            return Json(new { unitFare, totalFare }, JsonRequestBehavior.AllowGet);
        }

    }
}