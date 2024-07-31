using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Services;    
using Microsoft.AspNetCore.Mvc;
using MAMS.Models.ViewModels;

namespace MAMS.Controllers
{
    public class UsersController : BaseController
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(IConfiguration config, INotyfService notfy, ILogger<UsersController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
            : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            return View();
        }

        public async Task<IActionResult> Patient()
        {
            var (specializations, errorMessage) = await _specializationService.GetAllSpecializationsAsync();
            ViewBag.Specializaions = specializations;
            return View();
        }

        public async Task<IActionResult> Doctors()
        {
            IList<DoctorDetailsViewModel> doctors = new List<DoctorDetailsViewModel>();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                string user = _httpContextAccessor.HttpContext.Session.GetString("UserName");

                    var result = await _userService.GetDoctorsAsync();

                    if (result.Item1 != null)
                    {
                        doctors = result.Item1;
                    }
                    else
                    {
                        _notfy.Error(result.Item2);
                        return View();
                    }
                
                ViewData.Model = doctors;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }
            return View();
        }
    }
}
