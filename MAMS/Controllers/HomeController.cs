using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using MAMS.Services;
using MAMS.Models;
using MAMS.Models.ViewModels;

namespace MAMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService _userService;

        public HomeController(IConfiguration config, INotyfService notfy, ILogger<HomeController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            //_apiUrl = _config.GetSection("AppSettings")["ApiUrl"];
            _httpContextAccessor = contextAccessor;
            _userService = new UserService(appSettings.ApiUrl);

        }

        public async Task<IActionResult> Index()
        {
            string user =  HttpContext.Session.GetString("UserName");

            try
            {
                UserDetailsViewModel viewModel = new UserDetailsViewModel();

                if (user == null)
                {
                    return RedirectToAction("TimeOut", "Home");
                }
                else
                {
                    var result = await _userService.GetUserDetailsAsync(user);

                    if (result.Item1 != null)
                    {
                        viewModel = result.Item1;
                    }
                    else
                    {
                        _notfy.Error(result.Item2);
                        return RedirectToAction("Index");
                    }
                }
                ViewData.Model = viewModel;
            }
            catch (Exception ex)
            {
                _notfy.Error(ex.Message);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            string user = HttpContext.Session.GetString("UserName");
            ViewData["User"] = user;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}