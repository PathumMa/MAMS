using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using MAMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAMS.Controllers
{
    public class AvailabilityController : BaseController
    {
        private readonly ILogger<DoctorController> _logger;

        public AvailabilityController(IConfiguration config, INotyfService notfy, ILogger<DoctorController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
            : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int id)
        {
            IEnumerable<DoctorAvailableDetails> availabilities = new List<DoctorAvailableDetails>();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
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
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

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
