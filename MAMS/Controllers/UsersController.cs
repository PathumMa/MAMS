using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Services;    
using Microsoft.AspNetCore.Mvc;
using MAMS.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using MAMS.Models.ViewModels;

namespace MAMS.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        public readonly string _apiUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SpecializationService _specializationService;
        public LoginService _loginService;
        public UserService _userService;

        public UsersController(IConfiguration config, INotyfService notfy, ILogger<UsersController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            _apiUrl = appSettings.ApiUrl;
            _httpContextAccessor = contextAccessor;
            _specializationService = new SpecializationService(_apiUrl);
            _loginService = new LoginService(_apiUrl);
            _userService = new UserService(_apiUrl);
        }

        public IActionResult Index()
        {
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
            IList<DoctorDetailsViewModel> dt = new List<DoctorDetailsViewModel>();

            try
            {
                string user = _httpContextAccessor.HttpContext.Session.GetString("UserName");

                if(user != null)
                {
                    var result = await _userService.GetDoctorsAsync();

                    if (result.Item1 != null)
                    {
                        dt = result.Item1;
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
                ViewData.Model = dt;
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
