using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MAMS.Models;
using Newtonsoft.Json;

namespace MAMS.Services
{
    public class SpecializationService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public SpecializationService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient
            {
                BaseAddress = new Uri(_apiUrl)

            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(bool Success, string ErrorMessage)> AddSpecializationAsync(Specializations specialization)
        {
            try
            {


                HttpResponseMessage response = await _client.PostAsJsonAsync("Specialization/addSpec", specialization);

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
        public async Task<(Specializations, string)> GetSpecialization(int Id)
        {
            Specializations dt = new Specializations();
            string? errorMessage = null;

            try
            {

                    HttpResponseMessage response = await _client.GetAsync("Specialization/" + Id);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dt = JsonConvert.DeserializeObject<Specializations>(result);
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
        public async Task<(IList<Specializations>, string?)> GetAllSpecializationsAsync()
        {
            IList<Specializations> dt = new List<Specializations>();
            string? errorMessage = null;

            try
            {
                    HttpResponseMessage response = await _client.GetAsync("Specialization");

                    if (response.IsSuccessStatusCode)
                    {
                        string results = await response.Content.ReadAsStringAsync();
                        dt = JsonConvert.DeserializeObject<List<Specializations>>(results);
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

        public async Task<(bool Success, string ErrorMessage)> UpdateRecordStatusAsync(int Id, bool isChecked)
        {
            try
            {
                    var updatedRec = new Specializations
                    {
                        Specializations_Id = Id,
                        Record_Status = (Enums.ActiveStatus)(int)(isChecked ? Enums.ActiveStatus.Active : Enums.ActiveStatus.Inactive)
                    };

                    HttpResponseMessage response = await _client.PostAsJsonAsync<Specializations>("specialization/UpdateRecordStatus", updatedRec);

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

        public async Task<(bool Success, string ErrorMessage)> UpdateSpecializationAsync(Specializations specialization)
        {
            try
            {
                    HttpResponseMessage response = await _client.PutAsJsonAsync($"Specialization/updateSpec/{specialization.Specializations_Id}", specialization);

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

        public async Task<(bool Success, string ErrorMessage)> DeleteRecordAsync(int id)
        {
            try
            {
                    HttpResponseMessage response = await _client.DeleteAsync("Specialization/deleteSpec/" + id);

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



