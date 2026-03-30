using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        // Thêm dấu ? sau int hoặc gán = 1 để tránh lỗi null
        [ValidateInput(false)]
        public ActionResult Payment(string toStation, int? qty, string total, string ticketType = "single", string lang = "vn")
        {

            ViewBag.ToStation = toStation ?? "---";

            ViewBag.Quantity = qty ?? 1;

            ViewBag.Total = total ?? "0 VNĐ";

            ViewBag.Lang = lang;

            ViewBag.TicketType = ticketType ?? "single";

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

            // Khởi tạo tham số
            ReportParameter[] parameters = new ReportParameter[3];

            // 1. toStation gửi dạng Text (Nhà hát TP, Văn Thánh...)
            string decodedToStation = HttpUtility.HtmlDecode(toStation ?? "---");
            parameters[0] = new ReportParameter("pToStation", decodedToStation);

            // 2. pQty gửi dạng chuỗi số (RDLC sẽ tự ép về Integer như bạn đã chỉnh)
            // Dùng .ToString() để chuyển từ int sang string trước khi gửi
            parameters[1] = new ReportParameter("pQty", (qty ?? 1).ToString());

            // 3. pTotal gửi dạng Text (Bao gồm cả chữ VNĐ)
            parameters[2] = new ReportParameter("pTotal", total ?? "0 VNĐ");

            localReport.SetParameters(parameters);

            // ... phần xuất PDF giữ nguyên ...
            byte[] renderedBytes = localReport.Render("PDF");
            return File(renderedBytes, "application/pdf", "MetroTicket.pdf");
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