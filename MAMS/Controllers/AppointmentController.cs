using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using MAMS.Models.ViewModels;
using MAMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAMS.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly ILogger<DoctorController> _logger;

        public AppointmentController(IConfiguration config, INotyfService notfy, ILogger<DoctorController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
            : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IList<DoctorAvailabilityViewModel> doctors = new List<DoctorAvailabilityViewModel>();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var result = await _appointmentService.GetDoctorsAll();

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

        public async Task<IActionResult> Search()
        {
            var (activeSpecializations, errorMessage) = await _specializationService.GetAllSpecializationsAsync();

            ViewBag.Specializaions = activeSpecializations
                .Where(s => s.Record_Status == Enums.ActiveStatus.Active)
                .ToList();

            return View();
        }

        public async Task<IActionResult> SearchDoctors(string? name, string? specialization)
        {
            var (activeSpecializations, errorMessage) = await _specializationService.GetAllSpecializationsAsync();

            ViewBag.Specializaions = activeSpecializations
                .Where(s => s.Record_Status == Enums.ActiveStatus.Active)
                .ToList();

            IList<DoctorDetails> doctors = new List<DoctorDetails>();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var results = await _appointmentService.GetDoctorByName(name, specialization);

                if (results.Item1 != null)
                {
                    doctors = results.Item1;
                }
                else
                {
                    _notfy.Error(results.Item2);
                    return View("Search");
                }
                ViewData.Model = doctors;
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }
            return View("Search");
        }

        public async Task<IActionResult> Availabilities(int Id)
        {
            IList<DoctorAvailableDetails> doctors = new List<DoctorAvailableDetails>();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var result = await _availabilityService.AvailabilityAsync(Id);

                if (result.Item1 != null)
                {
                    doctors = result.Item1.Where(s => s.ActiveStatus == Enums.ActiveStatus.Active).ToList();
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

        public async Task<IActionResult> Booking(int Id, int DoctorId)
        {
            try
            {
                var (availability, errorMessage1) = await _availabilityService.GetAvailability(Id);

                if (availability == null)
                {
                    _notfy.Error("Availability Not Found!");
                    return View();
                }

                if (!Enum.TryParse(availability.Available_Day, true, out DayOfWeek dayOfWeek))
                {
                    _notfy.Error("Invalid Day of the Week!");
                    return View();
                }

                DateTime date = _calanderService.GetDateForDayOfWeek(dayOfWeek);
                var lastAppointmentNumber = await _appointmentService.GetLastAppointmentNumberAsync(Id, date);
                var yourAppointmentNumber = lastAppointmentNumber + 1;

                var appointmentCount = await _appointmentService.GetAppointmentCountAsync(Id, date);

                var doctorDetails = await _userService.GetDoctorDetailsByIdAsync(DoctorId);

                ViewBag.appointmentDate = date;
                ViewBag.YourAppointmentNumber = yourAppointmentNumber;
                ViewBag.AppointmentCount = appointmentCount;

                var doctorFee = doctorDetails.Item1.Doctor_Fee;
                var hospitalFee = doctorFee * 0.30m;
                var total = doctorFee + hospitalFee;

                ViewBag.doctorFee = doctorFee;
                ViewBag.hospitalFee = hospitalFee.ToString("F2");
                ViewBag.totalFee = total.ToString("F2");


                string? user = HttpContext.Session.GetString("UserName");

                if (user == null)
                {
                    _notfy.Warning("Please Log In.");
                    return View();
                }

                var (userDetails, errorMessage2) = await _userService.GetUserDetailsAsync(user);

                if (userDetails == null)
                {
                    _notfy.Error("user Details Not Found!");
                    return View();
                }

                var viewModel = new BookingViewModel
                {
                    UserTitle = userDetails.UserTitle,
                    Name = userDetails.First_Name + ' ' + userDetails.Last_Name,
                    Email = userDetails.Email,
                    PhoneNumber = userDetails.PhoneNumber,
                    PersonalId_Type = userDetails.PersonalId_Type,
                    Personal_Id = userDetails.Personal_Id,
                    Doctor_Id = availability.DoctorId,
                    Availability_Id = availability.Id,
                    Available_Day = availability.Available_Day,
                    Appointment_Date = date,
                    Appoinment_number = yourAppointmentNumber,
                    StartTime = availability.StartTime,
                    EndTime = availability.EndTime,
                    Doctor_fee = doctorFee,
                    Hospital_fee = hospitalFee,
                    Amount = total
                };

                return View(viewModel);
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning($"{ex.Message}", 5);
            }

            return View();
        }

        public async Task<IActionResult> PlaceNewAppointment(BookingViewModel bookingViewModel)
        {
            try
            {
                (bool success, string errorMessage) = await _appointmentService.AddAppointmentAsync(bookingViewModel);

                if (success)
                {
                    _notfy.Success($"Appointment Placed! - Appointment number :{bookingViewModel.Appoinment_number} - Appointment Date : {bookingViewModel.Appointment_Date} ");
                    return RedirectToAction("Search");
                }
                else
                {
                    await Booking(bookingViewModel.Availability_Id, bookingViewModel.Doctor_Id);
                    _notfy.Error(errorMessage, 5);
                    return View("Booking");
                }

            }
            catch (Exception ex)
            {
                await Booking(bookingViewModel.Availability_Id, bookingViewModel.Doctor_Id);
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return View("Booking");
            }
        }
    }




}
