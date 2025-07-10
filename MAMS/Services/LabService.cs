using AspNetCore;
using MAMS.Models;
using MAMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MAMS.Services
{
    public class LabService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public LabService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient()
            {
                BaseAddress = new Uri(_apiUrl)
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(bool Success, string ErrorMessage)> AddLabsAsync(LabTypeViewModel model)
        {
            try
            {

                HttpResponseMessage response = await _client.PostAsJsonAsync("Lab/addType", model);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(IList<LabTypeViewModel>, string?)> GetAllLabsAsync()
        {
            IList<LabTypeViewModel> dt = new List<LabTypeViewModel>();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("Lab/allTypes");

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    dt = JsonConvert.DeserializeObject<List<LabTypeViewModel>>(results);
                }
                else
                {
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (dt, errorMessage);
        }
        public async Task<(IList<LabTypeViewModel>, string?)> GetBySearch(string? labName, int? categoryId)
        {
            IList<LabTypeViewModel> labs = new List<LabTypeViewModel>();
            string? errorMessage = null;

            try
            {
                var query = new List<string>();

                if (!string.IsNullOrWhiteSpace(labName))
                {
                    query.Add($"labName={Uri.EscapeDataString(labName)}");
                }

                if (categoryId.HasValue)
                {
                    query.Add($"categoryId={categoryId.Value}");
                }

                string queryString = string.Join("&", query);
                string url = string.IsNullOrEmpty(queryString) ? "Lab/search" : $"Lab/search?{queryString}";

                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    labs = JsonConvert.DeserializeObject<List<LabTypeViewModel>>(results);
                }
                else
                {
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return (labs, errorMessage);
        }

        public async Task<(IList<LabCategoryViewModel>, string?)> GetAllLabCategoriesAsync()
        {
            IList<LabCategoryViewModel> dt = new List<LabCategoryViewModel>();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("Lab/categories");

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    dt = JsonConvert.DeserializeObject<List<LabCategoryViewModel>>(results);
                }
                else
                {
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (dt, errorMessage);
        }
        public async Task<(LabTypeViewModel? lab, string? errorMessage)> GetLabByIdAsync(int id)
        {
            LabTypeViewModel? lab = null;
            string? errorMessage = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync($"Lab/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    lab = JsonConvert.DeserializeObject<LabTypeViewModel>(result);
                }
                else
                {
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (lab, errorMessage);
        }
        public async Task<(bool success, string? errorMessage)> UpdateLabAsync(LabTypeViewModel updatedLab)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync($"Lab/update/{updatedLab.LabTypeId}", updatedLab);

                if (response.IsSuccessStatusCode)
                    return (true, null);

                string error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string? errorMessage)> DeleteLabAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("Lab/" + id);

                if (response.IsSuccessStatusCode)
                    return (true, null);

                string error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool success, string? errorMessage, LabBookingResponseViewModel? result)> BookLabAsync(LabBookingViewModel model)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("LabBooking/BookLab", model);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LabBookingResponseViewModel>(content);
                    return (true, null, result);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage, null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error booking lab: {ex.Message}");
            }
        }


    }
}