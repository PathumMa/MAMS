using AspNetCoreHero.ToastNotification.Abstractions;
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
    }
}
