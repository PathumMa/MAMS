using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models.ViewModels;
using MAMS.Services;
using MAMS.Services.Reports;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Threading.Tasks;

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
                var response = new LabBookingResponseViewModel
                {
                    ReferenceNo = result.ReferenceNo,
                    BookedDate = result.BookedDate,
                    Time = result.Time,
                    Status = result.Status,
                    LabName = result.LabName,
                    Price = result.Price,
                    Patient = result.Patient
                };

                TempData["Success"] = "Booking successful!";
                _notfy.Success("Booking successful!");

                TempData["BookingResult"] = JsonConvert.SerializeObject(response);

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lab booking");
                TempData["Error"] = "An error occurred while processing your request. Please try again later.";
                _notfy.Error(TempData["Error"].ToString());
                return RedirectToAction("Booking", new { labTypeId = model.LabTypeId });
            }

        }

        public IActionResult Success()
        {
            if (TempData["BookingResult"] == null)
            {
                _notfy.Error("No Booking Reults");
                return RedirectToAction("Booking");
            }

            var response = JsonConvert.DeserializeObject<LabBookingResponseViewModel>(TempData["BookingResult"].ToString());
            return View(response);
        }

        public IActionResult MyBookings()
        {
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            return View();
        }
        public async Task<IActionResult> PrintSlip(string refNo)
        {
            var (result, errorMessage) = await _labService.GetLabByRefNo(refNo);

            if(result == null)
            {
                _notfy.Error(errorMessage ?? "Booking not found");
                return RedirectToAction("Success");
            }
            
            var model = new LabResultViewModel
            {
                ReferenceNo = result.ReferenceNo,
                BookedDate = result.BookedDate,
                TimeSlot = result.TimeSlot,
                LabTypeName = result.LabTypeName,
                PatientName = result.PatientName,
                BookedPrice = result.BookedPrice
            };

            var stream = new MemoryStream();

            var document = new BookingSlipGenerator(model); // you’ll create this class
            document.GeneratePdf(stream);

            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", $"Booking_{refNo}_{DateTime.Now}.pdf");
        }
        

    }
}

