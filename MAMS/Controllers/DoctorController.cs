using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using MAMS.Models.ViewModels;
using MAMS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MAMS.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SpecializationService _specializationService;
        public LoginService _loginService;
        public UserService _userService;
        public AvailabilityService _availabilityService;

        public DoctorController(IConfiguration config, INotyfService notfy, ILogger<DoctorController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            _httpContextAccessor = contextAccessor;
            _specializationService = new SpecializationService(appSettings.ApiUrl);
            _loginService = new LoginService(appSettings.ApiUrl);
            _userService = new UserService(appSettings.ApiUrl);
            _availabilityService = new AvailabilityService(appSettings.ApiUrl);
        }

        public async Task<IActionResult> Index()
        {
            IList<DoctorDetailsViewModel> doctors = new List<DoctorDetailsViewModel>();

            try
            {
                string user = _httpContextAccessor.HttpContext.Session.GetString("UserName");
                ViewData.Model = _userService.GetUserDetailsAsync(user);

                if (user != null)
                {
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
                }
                else
                {
                    _notfy.Warning("Session Timeout!:", 5);
                    return View("TimedOut", "Home");
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
