using AspNetCore;
using MAMS.Models;
using MAMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MAMS.Services
{
    public class AppointmentService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public AppointmentService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient()
            {
                BaseAddress = new Uri(_apiUrl)
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(IList<DoctorAvailabilityViewModel>, string)> GetDoctorsAll()
        {
            IList<DoctorAvailabilityViewModel> doctors = new List<DoctorAvailabilityViewModel>();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync("Doctor/DoctorsAll");

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    doctors = JsonConvert.DeserializeObject<List<DoctorAvailabilityViewModel>>(results);
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
            return (doctors, errorMessage);

        }

        public async Task<(IList<DoctorDetails>, string)> GetDoctorByName(string? name,  string? specialization)
        {
            IList<DoctorDetails> doctors = new List<DoctorDetails>();
            string? errorMessage = null;

            try
            {
                var query = new List<string>();

                if (!string.IsNullOrEmpty(name))
                {
                    query.Add($"name={Uri.EscapeDataString(name)}");
                }
                if (!string.IsNullOrEmpty(specialization))
                {
                    query.Add($"specialization={Uri.EscapeDataString(specialization)}");
                }
                string queryString = string.Join("&", query);

                HttpResponseMessage response = await _client.GetAsync($"Doctor/DoctorByName?{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    doctors = JsonConvert.DeserializeObject<List<DoctorDetails>>(results);
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
            return (doctors, errorMessage);
        
        }

        public async Task<int> GetLastAppointmentNumberAsync(int doctorId, DateTime date)
        {
            try
            {
                var formattedDate = date.ToString("yyyy-MM-dd");

                HttpResponseMessage response = await _client.GetAsync($"Appointment/LastNumber?doctorId={doctorId}&date={formattedDate}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var lastNumber = JsonConvert.DeserializeObject<int>(result);

                    if (lastNumber == null)
                    {
                        return 0;
                    }
                    return lastNumber;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }

        public async Task<int> GetAppointmentCountAsync(int doctorId, DateTime date)
        {
            try
            {
                
                var formattedDate = date.ToString("yyyy-MM-dd");
                HttpResponseMessage response = await _client.GetAsync($"Appointment/AppoinmentCount?doctorId={doctorId}&date={formattedDate}");

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    var result = await response.Content.ReadAsStringAsync();

                    // Deserialize the response into an integer (the appointment count)
                    if (int.TryParse(result, out int appointmentCount))
                    {
                        return appointmentCount;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }

            return 0;
        }

        public async Task<(bool success, string errorMessage)> AddAppointmentAsync(BookingViewModel bookingViewModel)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("Appointment/Booking", bookingViewModel);

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
    }
}