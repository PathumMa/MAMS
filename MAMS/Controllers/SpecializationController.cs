using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using MAMS.Tools;

namespace MAMS.Controllers
{
    public class SpecializationController : Controller
    {
        private readonly ILogger<SpecializationController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        public readonly string apiUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SpecializationController(IConfiguration config, INotyfService notfy, ILogger<SpecializationController> logger, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            apiUrl = _config.GetSection("AppSettings")["ApiUrl"];
            _httpContextAccessor = contextAccessor;

        }

        public async Task<IActionResult> Index()
        {
            //calling the web API and populating the data in view using Entity Model Class

            IList<Specializations> dt = new List<Specializations>();
            try
            {
                _httpContextAccessor.HttpContext.Session.GetString("UserName");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.GetAsync("Specialization");

                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        dt = JsonConvert.DeserializeObject<List<Specializations>>(results);
                        _notfy.Success("view loaded Successfully.", 5);
                    }
                    else
                    {
                        _notfy.Error("Error clling web API!", 5);
                        return View();
                    }
                }
                string successMessage = TempData["SuccessMessage"] as string;
                string warningMessage = TempData["WarningMessage"] as string;
                string errorMessage = TempData["ErrorMessage"] as string;

                // Display TempData messages using INotyfService
                if (!string.IsNullOrEmpty(successMessage))
                {
                    _notfy.Success(successMessage);
                }
                if (!string.IsNullOrEmpty(warningMessage))
                {
                    _notfy.Warning(warningMessage);
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _notfy.Error(errorMessage);
                }

                return View(dt);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Warning("Session Timeout!:", 5);
                return View("TimedOut");
            }
        }


        public async Task<IActionResult> Create(Specializations NewSpec)
        {


            Specializations specializations = new Specializations()
            {
                Specializations_Name = NewSpec.Specializations_Name,
                Description = NewSpec.Description,
            };

            try
            {
                if (ModelState.IsValid && NewSpec.Specializations_Name != null)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage getData = await client.PostAsJsonAsync<Specializations>("Specialization/addSpec", specializations);

                        if (getData.IsSuccessStatusCode)
                        {
                            _notfy.Success($"New Specialization added Successfully!. {NewSpec.Specializations_Name}");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var errorMessage = await getData.Content.ReadAsStringAsync();
                            _notfy.Warning(errorMessage);
                            _notfy.Error("Creation Fail!.", 5);
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Update(Specializations specialization)
        {
            if (!ModelState.IsValid)
            {
                // Handle validation errors before making the API call
                // You may choose to display error messages to the user or take other actions
                return View(specialization); // Assuming you have a corresponding Update view
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PutAsJsonAsync<Specializations>("specialization/updateSpec/{specializations.Specializations_Id}", specialization);

                    if (getData.IsSuccessStatusCode)
                    {
                        _notfy.Success($"{specialization.Specializations_Name} Updated successfully.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorMessage = await getData.Content.ReadAsStringAsync();
                        _notfy.Warning(errorMessage);
                        _notfy.Error("update Fail!.", 5);
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> UpdateRec(int Id, bool isChecked)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var updatedRec = new Specializations
                    {
                        Specializations_Id = Id,
                        Record_Status = (Enums.ActiveStatus)(int)(isChecked ? Enums.ActiveStatus.Active : Enums.ActiveStatus.Inactive)
                    };

                    HttpResponseMessage getData = await client.PostAsJsonAsync<Specializations>("specialization/UpdateRecordStatus", updatedRec);

                    if (getData.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Updated Successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorMessage = await getData.Content.ReadAsStringAsync();
                        TempData["WarningMessage"] = errorMessage;
                        TempData["ErrorMessage"] = "update Fail!.";
                        TempData.Keep();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return RedirectToAction("Index");
            }
        }
    }

}