using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using MAMS.Models.ViewModels;
using MAMS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MAMS.Controllers
{
    public class DoctorController : BaseController
    {
        private readonly ILogger<DoctorController> _logger;
        public DoctorController(IConfiguration config, INotyfService notfy, ILogger<DoctorController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings) 
            : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IList<DoctorDetailsViewModel> doctors = new List<DoctorDetailsViewModel>();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
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

                ViewData.Model = doctors;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }
            return View();
        }

        public async Task<IActionResult> Profile(string userName)
        {
            DoctorDetailsViewModel doctor = new DoctorDetailsViewModel();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var result = await _userService.GetDoctorDetailsAsync(userName);

                if (result.Item1 != null)
                {
                    doctor = result.Item1;
                }
                else
                {
                    _notfy.Error(result.Item2);
                    return View();
                }
                ViewData.Model = doctor;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }
            return View();
        }

        public async Task<IActionResult> Edit(string Doctor)
        {
            DoctorDetailsViewModel doctorDetailsViewModel = new DoctorDetailsViewModel();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var (activeSpecializations, errorMessage) = await _specializationService.GetAllSpecializationsAsync();

                ViewBag.Specializaions = activeSpecializations
                    .Where(s => s.Record_Status == Enums.ActiveStatus.Active)
                    .ToList();

                var result = await _userService.GetDoctorDetailsAsync(Doctor);

                if (result.Item1 != null)
                {
                    doctorDetailsViewModel = result.Item1;
                }
                else
                {
                    _notfy.Error(result.Item2);
                    return View();
                }
                ViewData.Model = doctorDetailsViewModel;
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
