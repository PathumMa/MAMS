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

        public async Task<IActionResult> Index(int id)
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
                ViewData["DoctorId"] = id;
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
                        _notfy.Success($"{doctorAvailableDetails.Available_Day}, Updated successfully.");
                        //return View("Index");
                    }
                    else
                    {
                        _notfy.Warning(errorMessage);
                        _notfy.Error("update Fail!.", 5);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
            }

            ViewData["DoctorId"] = doctorAvailableDetails.DoctorId;
            var availabilities = await _availabilityService.AvailabilityAsync(doctorAvailableDetails.DoctorId);
            return View("Index", availabilities.Item1 ?? new List<DoctorAvailableDetails>());
        }

        public async Task<IActionResult> AddNew(DoctorAvailableDetails newAvailability)
        {

            DoctorAvailableDetails doctorAvailableDetails = new DoctorAvailableDetails()
            {
                DoctorId = newAvailability.DoctorId,
                Available_Day = newAvailability.Available_Day,
                StartTime = newAvailability.StartTime,
                EndTime = newAvailability.EndTime
            };

            try
            {
                if (HttpContext.Session.GetString("UserName") != null)
                {
                    if (ModelState.IsValid && newAvailability.DoctorId > 0)
                    {
                        (bool success, string errorMessage) = await _availabilityService.AddAvailabilityAsync(doctorAvailableDetails);

                        if (success)
                        {
                            _notfy.Success("Availability Added Successfully!.");
                        }
                        else
                        {
                            _notfy.Error("Adding Fail!", 5);
                            _notfy.Warning(errorMessage);
                            ModelState.AddModelError(string.Empty, errorMessage);
                        }
                    }
                    else
                    {
                        _notfy.Error("Invalid model state or invalid Doctor ID.");
                    }
                }
                else
                {
                    _notfy.Warning("Session Timeout!:", 5);
                    return View("TimedOut");
                }
            }
            catch (Exception ex)
            {
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
            }
            ViewData["DoctorId"] = doctorAvailableDetails.DoctorId;
            var availabilities = await _availabilityService.AvailabilityAsync(doctorAvailableDetails.DoctorId);
            return View("Index", availabilities.Item1 ?? new List<DoctorAvailableDetails>());
        }

        public async Task<IActionResult> Delete(int id, int doctorId)
        {
            try
            {
                if (doctorId > 0)
                {
                    var result = await _availabilityService.DeleteAvailabilityAsync(id);

                    if (result.success)
                    {
                        _notfy.Success("Delete Successfully!");
                    }
                    else
                    {
                        _notfy.Warning(result.ErrorMessage);
                    }
                }
                else
                {
                    _notfy.Error("Doctor Id required!. Please Contact your Admin.", 5);
                }
            }
            catch (Exception ex)
            {
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
            }

            ViewData["DoctorId"] = doctorId;
            var availabilities = await _availabilityService.AvailabilityAsync(doctorId);
            return View("Index", availabilities.Item1 ?? new List<DoctorAvailableDetails>());

        }
    }
}
