using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using MAMS.Services;

namespace MAMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        public readonly string _apiUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IConfiguration config, INotyfService notfy, ILogger<LoginController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            _apiUrl = appSettings.ApiUrl;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        public async Task<IActionResult> Enter(LoginViewModel user)
        {
            Suser dt = new Suser();

            if (user.UserName != null && user.Password != null)
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(_apiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage getData = await client.PostAsJsonAsync("Login/login", user);

                        if (getData.IsSuccessStatusCode)
                        {
                            string results = await getData.Content.ReadAsStringAsync();

                            dt = JsonConvert.DeserializeObject<Suser>(results);
                            var Id = dt.Id;
                            _httpContextAccessor.HttpContext.Session.SetString("UserName", dt.UserName);
                            _httpContextAccessor.HttpContext.Session.SetInt32("UserId", dt.Id);
                            _notfy.Success("view loaded Successfully.", 5);
                            return RedirectToAction("Index", "Home", new { Id = Id });

                        }
                        else if (getData.StatusCode != HttpStatusCode.OK)
                        {
                            var errorMessage = await getData.Content.ReadAsStringAsync();
                            _notfy.Warning(errorMessage);
                            return View("Login");
                        }
                        else
                        {
                            _notfy.Error("Error calling web API!", 5);
                            return View("Login");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        // Log the exception or handle it appropriately
                        _notfy.Error($"Error calling web API: {ex.Message}", 5);
                        return View("Login");
                    }
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
            return View("Login");
        }

        public IActionResult SignUp()
        {
            return RedirectToAction();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _notfy.Warning("Session Cleared! ");
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
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_apiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage getData = await client.PostAsJsonAsync("Login/DoctorReg", userRegistrationModel);

                        // Check if the request was successful
                        if (getData.IsSuccessStatusCode)
                        {
                            // User registration successful
                            _notfy.Success("You Registered Succesully !");
                            return RedirectToAction("SignUp");

                        }
                        else if (getData.StatusCode == HttpStatusCode.BadRequest)
                        {
                            var errorMessage = await getData.Content.ReadAsStringAsync();
                            _notfy.Warning(errorMessage);
                            return RedirectToAction("SignUp");
                        }
                        else
                        {
                            // Handle API error response
                            var errorMessage = await getData.Content.ReadAsStringAsync();
                            ModelState.AddModelError(string.Empty, $"API Error: {errorMessage}");
                            _notfy.Warning(errorMessage);
                            return RedirectToAction("SignUp", userRegistrationModel);

                        }
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
            else
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_apiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage getData = await client.PostAsJsonAsync("Login/UserReg", userRegistrationModel);

                        // Check if the request was successful
                        if (getData.IsSuccessStatusCode)
                        {
                            // User registration successful
                            _notfy.Success("You Registered Succesully !");
                            return RedirectToAction("SignUp");

                        }
                        else if (getData.StatusCode == HttpStatusCode.BadRequest)
                        {
                            var errorMessage = await getData.Content.ReadAsStringAsync();
                            _notfy.Warning(errorMessage);
                            return RedirectToAction("SignUp");
                        }
                        else
                        {
                            // Handle API error response
                            var errorMessage = await getData.Content.ReadAsStringAsync();
                            ModelState.AddModelError(string.Empty, $"API Error: {errorMessage}");
                            _notfy.Warning(errorMessage);
                            return RedirectToAction("SignUp", userRegistrationModel);

                        }
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
            
        }

    }
}