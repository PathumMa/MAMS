using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models.ViewModels;
using MAMS.Services;
using Microsoft.AspNetCore.Mvc;

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
            var (activeLabCategories, errorMessage2) = await _labService.GetAllLabCategoriesAsync();

            ViewBag.Categories = activeLabCategories
                .ToList();

            return View();
        }

        public async Task<IActionResult> SearchLabs(string? labName, int? categoryId)
        {
            var (activeLabCategories, errorMessage2) = await _labService.GetAllLabCategoriesAsync();
            ViewBag.Categories = activeLabCategories.ToList();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            IList<LabTypeViewModel> labs = new List<LabTypeViewModel>();

            try
            {
                var (resultLabs, errorMessage) = await _labService.GetBySearch(labName, categoryId);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _notfy.Error(errorMessage);
                    return View("Index");
                }

                labs = resultLabs;
                ViewData.Model = labs;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }

            return View("Index");
        }
        public async Task<IActionResult> Booking(int labTypeId)
        {
            var (labType, errorMessage) = await _labService.GetLabByIdAsync(labTypeId); // API call to get LabType details

            if (labType == null || !string.IsNullOrEmpty(errorMessage))
            {
                TempData["Error"] = "Invalid lab test selected.";
                _notfy.Error("Invalid lab test selected.");
                return RedirectToAction("SearchLabs", "LabBooking");
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
            try {
                var (success, errorMessage, result) = await _labService.BookLabAsync(model);

                if (!success || result == null)
                {
                    TempData["Error"] = errorMessage ?? "Booking failed. Try again.";
                    _notfy.Error(TempData["Error"].ToString());
                    return RedirectToAction("Booking", new { labTypeId = model.LabTypeId });
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
    }
}

