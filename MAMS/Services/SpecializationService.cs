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

        public SpecializationService(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<(bool Success, string ErrorMessage)> AddSpecializationAsync(Specializations specialization)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync("Specialization/addSpec", specialization);

                    if (getData.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }
                    else
                    {
                        var errorMessage = await getData.Content.ReadAsStringAsync();
                        return (false, errorMessage);
                    }
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
            string errorMessage = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.GetAsync("Specialization/" + Id);

                    if (getData.IsSuccessStatusCode)
                    {
                        string result = getData.Content.ReadAsStringAsync().Result;
                        dt = JsonConvert.DeserializeObject<Specializations>(result);
                    }
                    else
                    {
                        errorMessage = await getData.Content.ReadAsStringAsync();
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
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
                        errorMessage = await getData.Content.ReadAsStringAsync();
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
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
                        return (true, null);
                    }
                    else
                    {
                        var errorMessage = await getData.Content.ReadAsStringAsync();
                        return (false, errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false,  ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateSpecializationAsync(Specializations specialization)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PutAsJsonAsync($"Specialization/updateSpec/{specialization.Specializations_Id}", specialization);

                    if (getData.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }
                    else
                    {
                        var errorMessage = await getData.Content.ReadAsStringAsync();
                        return (false, errorMessage);
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl); 
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.DeleteAsync("Specialization/deleteSpec/" + id);

                    if (getData.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }
                    else
                    {
                        var errorMessage = await getData.Content.ReadAsStringAsync();
                        return (false, errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}



