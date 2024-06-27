using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using MAMS.Services;
using MAMS.Models.ViewModels;

namespace MAMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        public readonly string _apiUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SpecializationService _specializationService;
        public LoginService _loginService;
        public UserService _userService;

        public LoginController(IConfiguration config, INotyfService notfy, ILogger<LoginController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
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

        public async Task<IActionResult> Login()
        {
            return View();
        }

        public async Task<IActionResult> Enter(LoginViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (success, errorMessage) = await _loginService.LoginAsync(user);

                    if(success)
                    {
                        UserDetailsViewModel dt = new UserDetailsViewModel();
                        (dt, errorMessage) = await _userService.GetUserDetailsAsync(user.UserName);

                        _httpContextAccessor.HttpContext.Session.SetString("UserName", dt.UserName);

                        _notfy.Success("view loaded Successfully.", 5);
                        return RedirectToAction("Index", "Home", new { dt.UserName });
                        
                    }
                    else
                    {
                        _notfy.Warning(errorMessage);
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    if (user.UserName == null && user.Password == null)
                    {
                        _notfy.Warning("User Name and Password required");
                    }
                    else if (user.UserName == null)
                    {
                        _notfy.Warning("User Name required");
                    }
                    else // suser.Password == null
                    {
                        _notfy.Warning("Password required");
                    }
                }
            }
            catch (Exception ex)
            {
                _notfy.Information($"{ex.Message}");
                return RedirectToAction("Login");
            }
            return View("Login");
        }

        public async Task<IActionResult> SignUp()
        {
            var (activeSpecializations, errorMessage) = await _specializationService.GetAllSpecializationsAsync();

            ViewBag.Specializaions = activeSpecializations
                .Where(s => s.Record_Status == Enums.ActiveStatus.Active)
                .ToList();

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _notfy.Information("Session Cleared! ");
            return RedirectToAction("Login");
        }

        //view All records
        public async Task<IActionResult> Index()
        {
            //calling the web API and populating the data in view using Entity Model Cla
            IList<Suser> dt = new List<Suser>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("Login");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<List<Suser>>(results);

                }
                else
                {
                    _notfy.Error("Error clling web API!", 5);
                    return View();
                }
                ViewData.Model = dt;
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(SignUpViewModel userRegistrationModel)
        {
            if(userRegistrationModel.RoleId == 30)
            {
                try
                {
                    (bool success, string errorMessage) = await _loginService.DoctorRegister(userRegistrationModel);

                    if(success)
                    {
                        _notfy.Success($"{userRegistrationModel.UserName} Registered Succesully !");
                    }
                    else
                    {
                        _notfy.Error(errorMessage, 5);
                        return View("SignUp");
                    }
                }
                catch (Exception ex)
                {
                    // Handle other exceptions (e.g., network issues, etc.)
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                    _notfy.Error($"Error calling web API: {ex.Message}", 5);
                    return View("SignUp");
                }

                return RedirectToAction("SignUp");
            }
            else
            {
                try
                {
                    (bool success, string errorMessage) = await _loginService.UserRegister(userRegistrationModel);

                    if (success)
                    {
                        _notfy.Success($"{userRegistrationModel.UserName} Registered Succesully !");
                    }
                    else
                    {
                        _notfy.Error(errorMessage, 5);
                        return View("SignUp");
                    }
                }
                catch (Exception ex)
                {
                    // Handle other exceptions (e.g., network issues, etc.)
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                    _notfy.Error($"Error calling web API: {ex.Message}", 5);
                    return View("SignUp");
                }

            }

            return RedirectToAction("SignUp");
        }

    }
}