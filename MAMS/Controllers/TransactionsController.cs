using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models.ViewModels;
using MAMS.Services;
using MAMS.Services.Reports;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace MAMS.Controllers
{
    public class TransactionsController : BaseController
    {
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(IConfiguration config, INotyfService notfy, ILogger<TransactionsController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyLabBookings()
        {
            return View();
        }
        public async Task<IActionResult> SearchReceipt(string refNo)
        {
            var receipt = await _transactionService.GetReceiptByRefNoAsync(refNo);

            if (receipt == null)
            {
                TempData["Error"] = "Receipt not found";
                _notfy.Error("Receipt not found. Please check the reference number and try again.");
                return View("MyLabBookings"); // or RedirectToAction("Search")
            }

            return View("MyLabBookingReceipt", receipt);
        }
        public async Task<IActionResult> DownloadReceipt(string refNo)
        {
            var receipt = await _transactionService.GetReceiptByRefNoAsync(refNo);

            if (receipt == null)
                return NotFound();

            var stream = new MemoryStream();

            var document = new LabReceiptPdfDocument(receipt); // you’ll create this class
            document.GeneratePdf(stream);

            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", $"{receipt.ReferenceNo}.pdf");
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
    }
}
