using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models.ViewModels;
using MAMS.Services;
using MAMS.Services.Reports;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MAMS.Controllers
{
    public class LabBookingController : BaseController
    {
        private readonly ILogger<LabBookingController> _logger;

        public LabBookingController(IConfiguration config, INotyfService notfy, ILogger<LabBookingController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;

        }
        public async Task<IActionResult> Index()
        {
            var (activeLabCategories, errorMessage) = await _labService.GetAllLabCategoriesAsync();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _notfy.Error(errorMessage);
            }
            ViewBag.Categories = activeLabCategories.ToList();

            return View();
        }

        public async Task<IActionResult> SearchLabs(string? labName, int? categoryId)
        {
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            var (activeLabCategories, errorMessage2) = await _labService.GetAllLabCategoriesAsync();
            ViewBag.Categories = activeLabCategories?.ToList() ?? new List<LabCategoryViewModel>();

            IList<LabTypeViewModel> labs = new List<LabTypeViewModel>();

            try
            {
                var (resultLabs, errorMessage) = await _labService.GetBySearch(labName, categoryId);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _notfy.Error(errorMessage);
                    return View("Index");
                }

                labs = resultLabs ?? new List<LabTypeViewModel>();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }

            return View("Index", labs);
        }
        public async Task<IActionResult> Booking(int labTypeId)
        {
            var (labType, errorMessage) = await _labService.GetLabByIdAsync(labTypeId); // API call to get LabType details

            if (labType == null || !string.IsNullOrEmpty(errorMessage))
            {
                TempData["Error"] = "Invalid lab test selected.";
                _notfy.Error("Invalid lab test selected.");
                return RedirectToAction("SearchLabs");
            }

            var model = new LabBookingViewModel
            {
                LabTypeId = labType.LabTypeId

            };

            ViewBag.LabName = labType.LabName;
            ViewBag.Price = labType.Price;

            return View(model);
        }
        public async Task<IActionResult> CreateBooking(LabBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload lab name and price for the form in case of error
                var (labType, err) = await _labService.GetLabByIdAsync(model.LabTypeId);
                ViewBag.LabName = labType?.LabName ?? "";
                ViewBag.Price = labType?.Price ?? 0;
                return View("Booking", model);
            }

            try {
                var (success, errorMessage, result) = await _labService.BookLabAsync(model);

                if (!success || result == null)
                {
                    TempData["Error"] = errorMessage ?? "Booking failed. Try again.";
                    _notfy.Error(TempData["Error"].ToString());

                    // Reload lab name and price for the form in case of error
                    var (labType, err) = await _labService.GetLabByIdAsync(model.LabTypeId);
                    ViewBag.LabName = labType?.LabName ?? "";
                    ViewBag.Price = labType?.Price ?? 0;

                    return View("Booking", model);
                }

                TempData["Success"] = "Booking successful!";
                TempData["BookedDate"] = result.BookedDate;
                TempData["Time"] = result.Time;
                TempData["LabName"] = result.LabName;
                TempData["Price"] = result.Price.ToString("F2");
                TempData["Patient"] = result.Patient;
                _notfy.Success("Booking successful!");

                return RedirectToAction("Success", new { refNo = result.ReferenceNo });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lab booking");
                TempData["Error"] = "An error occurred while processing your request. Please try again later.";
                _notfy.Error(TempData["Error"].ToString());
                return RedirectToAction("Booking", new { labTypeId = model.LabTypeId });
            }

        }

        public IActionResult Success(string refNo)
        {
            ViewBag.RefNo = refNo;
            return View();
        }

        public IActionResult MyBookings()
        {
            return View();
        }
        public IActionResult PrintSlip(string refNo)
        {
            // You can fetch full details from TempData or session or a service.
            var model = new LabBookingResponseViewModel
            {
                ReferenceNo = refNo,
                Patient = TempData["Patient"]?.ToString(),
                LabName = TempData["LabName"]?.ToString(),
                BookedDate = TempData["BookedDate"]?.ToString(),
                Time = TempData["Time"]?.ToString(),
                Price = decimal.Parse(TempData["Price"]?.ToString() ?? "0")
            };

            var generator = new BookingSlipGenerator();
            var pdf = generator.Generate(model);

            return File(pdf, "application/pdf", $"Booking_{refNo}.pdf");
        }
        public IActionResult DownloadSample()
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A5);

                    page.Header().Text("MAMS - Lab Booking Slip")
                        .FontSize(20).Bold().FontColor(Colors.Blue.Medium);

                    page.Content().Column(col =>
                    {
                        col.Spacing(5);
                        col.Item().Text("Patient Name: Test Patient");
                        col.Item().Text("Lab Test: Blood Test");
                        col.Item().Text("Date: " + DateTime.Now.ToString("dd/MM/yyyy"));
                        col.Item().Text("Time: 10:30 AM");
                        col.Item().Text("Fee: Rs. 1500.00");
                    });

                    page.Footer().AlignCenter().Text("MAMS | Powered by Pathum")
                        .FontSize(10).FontColor(Colors.Grey.Darken1);
                });
            });

            byte[] pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", "LabBookingSlip.pdf");
        }

    }
}

