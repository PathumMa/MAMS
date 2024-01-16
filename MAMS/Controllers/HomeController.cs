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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IConfiguration config, INotyfService notfy, ILogger<HomeController> logger, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            apiUrl = _config.GetSection("AppSettings")["ApiUrl"];
            _httpContextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index(int Id)
        {
            UserDetailsViewModel viewModel = new UserDetailsViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("User/" + Id);

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    viewModel = JsonConvert.DeserializeObject<UserDetailsViewModel>(results);
                }
                else
                {
                    _notfy.Error("Error clling web API!", 5);
                    return View();
                }
                ViewData.Model = viewModel;
            }
            return View(viewModel);
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