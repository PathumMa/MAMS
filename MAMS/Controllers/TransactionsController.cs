using AspNetCoreHero.ToastNotification.Abstractions;
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
    }
}
