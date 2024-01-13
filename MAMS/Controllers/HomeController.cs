using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace MAMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        public readonly string apiUrl;

        public HomeController(IConfiguration config, INotyfService notfy, ILogger<HomeController> logger)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            apiUrl = _config.GetSection("AppSettings")["ApiUrl"];
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}