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
    public class SpecializationController : BaseController
    {
        private readonly ILogger<SpecializationController> _logger;

        public SpecializationController(IConfiguration config, INotyfService notfy, ILogger<SpecializationController> logger, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        : base(config, notfy, contextAccessor, appSettings)
        {
            _logger = logger;

        }

        public async Task<IActionResult> Index()
        {
            //calling the web API and populating the data in view using Entity Model Class

            IList<Specializations> dt = new List<Specializations>();

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var result = await _specializationService.GetAllSpecializationsAsync();


                if (result.Item1 != null)
                {
                    dt = result.Item1;
                }
                else
                {
                    _notfy.Error(result.Item2);
                    return View();
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

        public async Task<IActionResult> Status()
        {
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            return View();
        }

        public IActionResult Create()
        {
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }


            return View();
        }

        public async Task<IActionResult> CreateNew(Specializations newSpec)
        {


            Specializations specializations = new Specializations()
            {
                Specializations_Name = newSpec.Specializations_Name,
                Description = newSpec.Description,
            };

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {

                if (ModelState.IsValid && newSpec.Specializations_Name != null)
                {

                    (bool success, string errorMessage) = await _specializationService.AddSpecializationAsync(specializations);

                    if (success)
                    {
                        _notfy.Success($"{newSpec.Specializations_Name}, Successfully Added  as a new Specialization!. ");
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

            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                var result = await _specializationService.GetSpecialization(Id);

                if (result.Item1 != null)
                {
                    dt = result.Item1;
                }
                else
                {
                    _notfy.Error(result.Item2);
                    return RedirectToAction("Index");
                }
                ViewData.Model = dt;
            }
            catch (Exception ex)
            {
                _notfy.Error($"Error calling service: {ex.Message}", 5);
                return RedirectToAction("Index");
            }
            return View();

        }

        public async Task<IActionResult> Update(Specializations currentSpec)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    (bool success, string errorMessage) = await _specializationService.UpdateSpecializationAsync(currentSpec);

                    if (success)
                    {
                        _notfy.Success($"{currentSpec.Specializations_Name}, Updated successfully.");
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
            if (!IsSessionValid())
            {
                return View("TimedOut", "Home");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    (bool success, string errorMessage) = await _specializationService.UpdateRecordStatusAsync(Id, isChecked);

                    if (success)
                    {
                        _notfy.Success($"Updated successfully.");
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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _specializationService.DeleteRecordAsync(id);

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