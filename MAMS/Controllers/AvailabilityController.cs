using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using MAMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAMS.Controllers
{
    public class AvailabilityController : Controller
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SpecializationService _specializationService;
        public LoginService _loginService;
        public UserService _userService;
        public AvailabilityService _availabilityService;

        public AvailabilityController(IConfiguration config, INotyfService notfy, ILogger<DoctorController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
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

        public async Task<IActionResult> Availabilities(int id)
        {
            IEnumerable<DoctorAvailableDetails> availabilities = new List<DoctorAvailableDetails>();

            try
            {
                string user = _httpContextAccessor.HttpContext.Session.GetString("UserName");

                if (user != null)
                {
                    var result = await _availabilityService.AvailabilityAsync(id);

                    if (result.Item1 != null)
                    {
                        availabilities = result.Item1;
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
                ViewData.Model = availabilities;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }
            return View();
        }

        public async Task<IActionResult> Update(DoctorAvailableDetails doctorAvailableDetails)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    (bool success, string errorMessage) = await _availabilityService.UpdateAvailabilityAsync(doctorAvailableDetails);

                    if (success)
                    {
                        _notfy.Success($"{doctorAvailableDetails.Day}, Updated successfully.");
                        return RedirectToAction("Availabilities");
                    }
                    else
                    {
                        _notfy.Warning(errorMessage);
                        _notfy.Error("update Fail!.", 5);
                        return View("Availabilities");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return View("Availabilities");
            }
            return View();
        }
    }
}
