using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using MAMS.ViewModels;

namespace MAMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        public readonly string apiUrl;

        public LoginController(IConfiguration config, INotyfService notfy, ILogger<LoginController> logger)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            apiUrl = _config.GetSection("AppSettings")["ApiUrl"];
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        //view All records
        public async Task<IActionResult> Index()
        {
            //calling the web API and populating the data in view using Entity Model Cla
            IList<Suser> dt = new List<Suser>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
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

        public async Task<ActionResult<LoginViewModel>> Enter(LoginViewModel user)
        {
            
            if (user.UserName != null && user.Password != null)
            {
                SuserDetails dt = new SuserDetails();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync("Login/login", user);

                    if (getData.IsSuccessStatusCode)
                    {
                        string results = await getData.Content.ReadAsStringAsync();

                        dt = JsonConvert.DeserializeObject<SuserDetails>(results);

                        return RedirectToAction("Index", "Home");
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
                    //ViewData.Model = dt;
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

        [HttpPost]
        public async Task<IActionResult> Register(SignUpViewModel userRegistrationModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync("Login/register", userRegistrationModel);

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
                return RedirectToAction("SignUp", userRegistrationModel);
            }
        }

    }
}