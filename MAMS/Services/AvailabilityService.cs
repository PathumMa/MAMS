using AspNetCore;
using MAMS.Models;
using MAMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MAMS.Services
{
    public class AvailabilityService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public AvailabilityService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient()
            {
                BaseAddress = new Uri(_apiUrl)
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(IList<DoctorAvailableDetails>, string)> GetAllAvailabilities()
        {
            IList<DoctorAvailableDetails> availabilities = new List<DoctorAvailableDetails>();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("Doctor/availabilities");

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    availabilities = JsonConvert.DeserializeObject<List<DoctorAvailableDetails>>(results);
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
            return (availabilities, errorMessage);
        }

        public async Task<(IList<DoctorAvailableDetails>, string)> AvailabilityAsync(int id)
        {
            IList<DoctorAvailableDetails> availabilities = new List<DoctorAvailableDetails>();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("Doctor/availabilities/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    availabilities = JsonConvert.DeserializeObject<List<DoctorAvailableDetails>>(results);
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
            return (availabilities, errorMessage);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateAvailabilityAsync(DoctorAvailableDetails doctorAvailableDetails)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync($"Doctor/UpdateAvailability/{doctorAvailableDetails.Id}", doctorAvailableDetails);

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

        public async Task<(bool Success, string ErrorMessage)> AddAvailabilityAsync(DoctorAvailableDetails newAvailability)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("Doctor/AddAvailability", newAvailability);

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

        public async Task<(bool success, string ErrorMessage)> DeleteAvailabilityAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("Doctor/DeleteAvailability/" + id);

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
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(DoctorAvailableDetails, string)> GetAvailability(int Id)
        {
            DoctorAvailableDetails availability = new DoctorAvailableDetails();
            string? errorMessage = null;

            try
            {

                HttpResponseMessage response = await _client.GetAsync("Doctor/availability/ " + Id);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    availability = JsonConvert.DeserializeObject<DoctorAvailableDetails>(result);
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
            return (availability, errorMessage);

        }
    }
}
