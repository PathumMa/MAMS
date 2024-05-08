using MAMS.Models;
using MAMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MAMS.Services
{
    public class AvailabilityService
    {
        private readonly HttpClient _httpClient;

        public AvailabilityService(IHttpClientFactory httpClientFactory, string apiUrl)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpClient.BaseAddress = new Uri(apiUrl);
        }

        public async Task<(IEnumerable<DoctorAvailableDetails>, string)> GetAllAvailabilities()
        {
            IList<DoctorAvailableDetails> availabilities = new List<DoctorAvailableDetails>();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("Doctor/availabilities");

                if (response.IsSuccessStatusCode)
                {
                    string results = response.Content.ReadAsStringAsync().Result;
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
    }
}
