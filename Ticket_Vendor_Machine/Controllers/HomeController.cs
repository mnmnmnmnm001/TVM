using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Ticket_Vendor_Machine.ServiceFunction;
using Microsoft.Reporting.WebForms;

namespace Ticket_Vendor_Machine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Payment(string toStation, int? qty, string total, string ticketType, string lang = "vn", string stEN = "")
        {
            ViewBag.ToStation = toStation ?? "---";
            ViewBag.Quantity = qty ?? 1;
            ViewBag.Total = total ?? "0 VNĐ";
            ViewBag.Lang = lang;
            ViewBag.TicketType = string.IsNullOrEmpty(ticketType) ? "single" : ticketType;
            ViewBag.StEN = string.IsNullOrEmpty(stEN) ? "---" : stEN;

            switch (ViewBag.TicketType as string)
            {
                case "1day":
                    ViewBag.TicketTypeLabel = "Vé 1 ngày";
                    ViewBag.ValidUntil = DateTime.Now.ToString("dd/MM/yy");
                    break;
                case "single":
                default:
                    ViewBag.TicketTypeLabel = "Vé một chặng";
                    ViewBag.ValidUntil = DateTime.Now.ToString("dd/MM/yy");
                    break;
            }
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Generate(string toStation, int? qty, string total)
        {
            var reportPath = Path.Combine(Server.MapPath("~/Reports/Ticket.rdlc"));
            LocalReport localReport = new LocalReport { ReportPath = reportPath };

            ReportParameter[] parameters = new ReportParameter[3];
            string decodedToStation = HttpUtility.HtmlDecode(toStation ?? "---");
            parameters[0] = new ReportParameter("pToStation", decodedToStation);
            parameters[1] = new ReportParameter("pQty", (qty ?? 1).ToString());
            parameters[2] = new ReportParameter("pTotal", total ?? "0 VNĐ");

            localReport.SetParameters(parameters);

            byte[] renderedBytes = localReport.Render("PDF");
            return File(renderedBytes, "application/pdf", "MetroTicket.pdf");
        }

        private readonly FareCalculator _fareCalc = new FareCalculator();

        [HttpGet]
        public JsonResult CalculateFare(int destinationId, int quantity = 1, string ticketType = "single")
        {
            if (quantity < 1) quantity = 1;
            if (quantity > 9) quantity = 9;

            // If user selects 1-day ticket, return fixed 30,000 VND per passenger
            if (!string.IsNullOrEmpty(ticketType) && ticketType == "1day")
            {
                decimal unitFare = 30000m;
                decimal totalFare = unitFare * quantity;
                return Json(new { unitFare, totalFare }, JsonRequestBehavior.AllowGet);
            }

            // Single-journey (default) behavior:
            decimal unit = _fareCalc.GetUnitFare(destinationId);
            decimal total = _fareCalc.GetTotalFare(destinationId, quantity);

            return Json(new { unitFare = unit, totalFare = total }, JsonRequestBehavior.AllowGet);
        }
    }
}