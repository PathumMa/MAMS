using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Reports.Documents;
using MAMS.Services;
using MAMS.Services.Reports;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace MAMS.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IConfiguration config, INotyfService notfy, ILogger<ReportsController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;

        }

        public async Task<IActionResult> Index()
        {
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            return View();
        }

        public async Task<IActionResult> DownloadReceiptPdf(string refNo)
        {
            if (string.IsNullOrEmpty(refNo))
            {
                return BadRequest("Reference number is required.");
            }

            var receiptStream = await _transactionService.DownloadReceiptPdfAsync(refNo);

            if (receiptStream == null)
            {
                return NotFound("No receipt found for the given reference number.");
            }

            // Return the file as a PDF download
            return File(receiptStream, "application/pdf", $"LabReceipt_{refNo}.pdf");
        }

        //Daily Lab Summery
        public async Task<IActionResult> LabDailyReport(DateTime? date)
        {
            if (date == null)
                date = DateTime.Today;

            var reportData = await _reportService.GetLabReportAsync(date.Value);
            if (reportData == null)
            {
                _notfy.Error("No lab bookings found for the selected date.");
            }

            return View(reportData);
        }
        public async Task<IActionResult> DownloadLabReportPdf(DateTime date)
        {
            var reportData = await _reportService.GetLabReportAsync(date);

            var document = new LabSummaryReportPdfDocument(reportData, date); // You can design this
            var pdfStream = document.GeneratePdf();

            return File(pdfStream, "application/pdf", $"LabDailyReport_{date:yyyyMMdd}.pdf");
        }

        //Doctor Appointment Summery
        public async Task<IActionResult> DoctorAppointmentSummary(DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;

            var report = await _reportService.GetDoctorAppointmentSummaryAsync(selectedDate); // your service call
            if(report == null || !report.Any())
            {
                _notfy.Error("No Doctor Appoinments found for the selected date.");
            }

            ViewBag.SelectedDate = selectedDate;
            return View(report);
        }
        public async Task<IActionResult> DownloadDoctorAppointmentSummaryPdf(DateTime date)
        {
            var reportData = await _reportService.GetDoctorAppointmentSummaryAsync(date);

            if (reportData == null || !reportData.Any())
            {
                _notfy.Warning("No data available to generate PDF for the selected date.");
                return RedirectToAction("DoctorAppointmentSummary", new { date });
            }
            var document = new DoctorAppointmentSummaryPdfDocument(reportData, date); // QuestPDF document
            var pdfStream = document.GeneratePdf();

            return File(pdfStream, "application/pdf", $"DoctorAppointments_{date:yyyyMMdd}.pdf");
        }

        //Revenue Summary
        public async Task<IActionResult> RevenueSummary(DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;
            var report = await _reportService.GetRevenueSummaryAsync(selectedDate);

            if (report == null)
            {
                _notfy.Error("No revenue data found for the selected date.");
            }

            ViewBag.SelectedDate = selectedDate;
            return View(report);
        }

        public async Task<IActionResult> DownloadRevenueSummaryPdf(DateTime date)
        {
            var report = await _reportService.GetRevenueSummaryAsync(date);

            if (report == null)
                return NotFound("No data available to generate PDF.");

            var document = new RevenueSummaryPdfDocument(report, date);
            var stream = document.GeneratePdf();

            return File(stream, "application/pdf", $"RevenueSummary_{date:yyyyMMdd}.pdf");
        }






    }
}
