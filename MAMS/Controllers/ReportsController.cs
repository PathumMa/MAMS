using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models.ViewModels;
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

        public async Task<IActionResult> DownloadLabReport(string refNo)
        {
            var (result, errorMessage) = await _labService.GetLabByRefNo(refNo);
            if (result == null)
            {
                _notfy.Error("No Lab report found.");
                return RedirectToAction("GuestBookings", "LabBooking");
            }
                

            var model = new LabResultViewModel
            {
                ReferenceNo = result.ReferenceNo,
                BookedDate = result.BookedDate,
                TimeSlot = result.TimeSlot,
                LabTypeName = result.LabTypeName,
                PatientName = result.PatientName,
                BookedPrice = result.BookedPrice,
                ResultValue = result.ResultValue,
                Comments = result.Comments,
                Status = result.Status,
                PerformedDate = result.PerformedDate
            };

            var stream = new MemoryStream();

            var document = new LabReportPdfGenerator(result); // you’ll create this class
            document.GeneratePdf(stream);

            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", $"LAB_Report_{refNo}_{DateTime.Now}.pdf");
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

        //Lab Summery by Range
        public async Task<IActionResult> LabReportByRange(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
                startDate = DateTime.Today;
            if(endDate == null)
                endDate = DateTime.Today;

            var reportData = await _reportService.GetLabReportByRange(startDate.Value, endDate.Value);
            if (reportData == null)
            {
                _notfy.Error("No lab bookings found for the selected date.");
            }

            return View(reportData);
        }
        //public async Task<IActionResult> DownloadRangeLabReportPdf(DateTime startDate, DateTime endDate)
        //{
        //    var reportData = await _reportService.GetLabReportAsync(DateTime startDate, DateTime endDate);

        //    var document = new LabRangeReportPdfDocument(reportData, DateTime startDate, DateTime endDate);
        //    var pdfStream = document.GeneratePdf();

        //    return File(pdfStream, "application/pdf", $"LabDailyReport_{startDate:yyyyMMdd}.pdf");
        //}





    }
}
