using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using MAMS.Services;
using System.Reflection.Metadata.Ecma335;

namespace MAMS.Controllers
{
    public class SpecializationController : Controller
    {
        private readonly ILogger<SpecializationController> _logger;
        private readonly INotyfService _notfy;
        private readonly IConfiguration _config;
        public readonly string apiUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        public SpecializationService specializationService = new SpecializationService();


        public SpecializationController(IConfiguration config, INotyfService notfy, ILogger<SpecializationController> logger, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _config = config;
            _notfy = notfy;
            apiUrl = _config.GetSection("AppSettings")["ApiUrl"];
            _httpContextAccessor = contextAccessor;
            //_specializationService = specializationService;

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

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateNew(Specializations newSpec)
        {


            Specializations specializations = new Specializations()
            {
                Specializations_Name = newSpec.Specializations_Name,
                Description = newSpec.Description,
            };

            try
            {
                if (ModelState.IsValid && newSpec.Specializations_Name != null)
                {

                    (bool success, string errorMessage) = await specializationService.AddSpecializationAsync(specializations, apiUrl);

                    if (success)
                    {
                        _notfy.Success($"{newSpec.Specializations_Name} Added Successfully as a new Specialization!. ");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _notfy.Error("Creation Fail!", 5);
                        _notfy.Warning(errorMessage);
                        ModelState.AddModelError(string.Empty, errorMessage);
                        return View("Create");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return View("Create");
            }
            return View();
        }

        public async Task<IActionResult> Edit(int Id)
        {
            Specializations dt = new Specializations();

            var result = await specializationService.GetSpecialization(Id, apiUrl);

            try
            {
                if (result.Item1 != null)
                {
                    dt = result.Item1;
                }
                else
                {
                    _notfy.Error(result.Item2);
                    RedirectToAction("Index");
                }
                ViewData.Model = dt;
            }
            catch (Exception ex)
            {
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                RedirectToAction("Index");
            }
            return View();

        }

        public async Task<IActionResult> Update(Specializations currentSpec)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    (bool success, string errorMessage) = await specializationService.UpdateSpecializationAsync(currentSpec, apiUrl);

                    if (success)
                    {
                        _notfy.Success($"{currentSpec.Specializations_Name} Updated successfully.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _notfy.Warning(errorMessage);
                        _notfy.Error("update Fail!.", 5);
                        return View("Edit");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
                return View("Edit");
            }
            return View();
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await specializationService.DeleteRecordAsync(id, apiUrl);

                if (result.Success)
                {
                    _notfy.Success("Delete Successfully!");
                }
                else
                {
                    _notfy.Warning(result.ErrorMessage);
                    _notfy.Error("Deletion Fail!.", 5);
                }
            }
            catch (Exception ex)
            {
                _notfy.Error($"Error calling web API: {ex.Message}", 5);
            }
            return RedirectToAction("Index");
        }
    }
}