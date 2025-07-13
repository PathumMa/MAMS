using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using MAMS.Models.ViewModels;
using MAMS.Services;
using Microsoft.AspNetCore.Mvc;
using static MAMS.Services.Enums;

namespace MAMS.Controllers
{
    public class LabController : BaseController
    {
        private readonly ILogger<LabController> _logger;

        public LabController(IConfiguration config, INotyfService notfy, ILogger<LabController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;

        }

        public async Task<IActionResult> Index()
        {
            IList<LabTypeViewModel> dt = new List<LabTypeViewModel>();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var result = await _labService.GetAllLabsAsync();

                if (result.Item1 != null)
                {
                    dt = result.Item1;
                }
                else
                {
                    _notfy.Error(result.Item2);
                    return View();
                }

                ViewData.Model = dt;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }
            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!IsSessionValid())
                return View("TimedOut", "Home");

            var (activeLabCategories, errorMessage) = await _labService.GetAllLabCategoriesAsync();

            ViewBag.Categories = activeLabCategories
                .ToList();

            return View(new LabTypeViewModel());
        }

        public async Task<IActionResult> CreateNew(LabTypeViewModel model)
        {
            if (!IsSessionValid())
                return View("TimedOut", "Home");

            if (!ModelState.IsValid)
            {
                _notfy.Warning("Please fix validation errors.");
                await LoadCategoriesToViewBag(); // Load dropdown again
                return View("Create", model);
            }

            try
            {
                var (success, errorMessage) = await _labService.AddLabsAsync(model);

                if (success)
                {
                    _notfy.Success($"{model.LabName} successfully added!");
                    return RedirectToAction("Index");
                }

                _notfy.Error("Creation failed.");
                _notfy.Warning(errorMessage);
                await LoadCategoriesToViewBag();
                return View("Create", model);
            }
            catch (Exception ex)
            {
                _notfy.Error($"Unexpected error: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadCategoriesToViewBag();
                return View("Create", model);
            }
        }

        // Helper method
        private async Task LoadCategoriesToViewBag()
        {
            var (categories, _) = await _labService.GetAllLabCategoriesAsync();
            ViewBag.Categories = categories.Where(c => c.IsActive == Enums.ActiveStatus.Active).ToList();
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsSessionValid())
                return View("TimedOut", "Home");

            var (lab, errorMessage) = await _labService.GetLabByIdAsync(id);

            if (lab == null)
            {
                _notfy.Error($"Lab type with ID {id} not found.");
                return RedirectToAction("Index");
            }

            var (activeLabCategories, errorMessage2) = await _labService.GetAllLabCategoriesAsync();

            ViewBag.Categories = activeLabCategories
                .ToList();

            return View(lab);
        }

        public async Task<IActionResult> Update(LabTypeViewModel model)
        {
            if (!IsSessionValid())
                return View("TimedOut", "Home");

            try
            {
                if (ModelState.IsValid)
                {
                    (bool success, string? errorMessage) = await _labService.UpdateLabAsync(model);

                    if (success)
                    {
                        _notfy.Success($"{model.LabName}, Updated successfully.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _notfy.Warning(errorMessage);
                        _notfy.Error("update Fail!.", 5);
                        return View("Edit");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return View("Edit");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var result = await _labService.DeleteLabAsync(id);

                if (result.success)
                {
                    if (!string.IsNullOrEmpty(result.errorMessage))
                    {
                        _notfy.Warning("Lab type has bookings and was inactive instead of deleted.");
                    }
                    else
                    {
                        _notfy.Success($"Lab deleted successfully.");
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    _notfy.Error(result.Item2);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return RedirectToAction("Index");
            }
        }

        
        //Booked Labs performance
        public async Task<IActionResult> PerformLabs()
        {
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            return View();
        }

        public async Task<IActionResult> PerformBookedLabs(string referenceNo)
        {
            var (result, errorMessage) = await _labService.GetLabByRefNo(referenceNo);
            try
            {
                if (result == null)
                {
                    _notfy.Error(errorMessage);
                    return RedirectToAction("PerformLabs");
                }

            }
            catch (Exception ex)
            {
                _notfy.Error($"Error: {ex.Message}");
                return RedirectToAction("PerformLabs");
            }

            return View("PerformLabs", result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBookedLab(LabResultUpdateViewModel model)
        {

            var (result, errorMessage) = await _labService.UpdateLabResultAsync(model.LabResultId, model);

            if (result)
            {
                TempData["Success"] = "Lab result updated successfully.";
                _notfy.Success($"{model.ReferenceNo} result updated successfully.");
            }
            else
            {
                TempData["Error"] = "Update failed.";
                _notfy.Error(errorMessage);
            }

            return RedirectToAction("PerformLabs");
        }

    }
}
