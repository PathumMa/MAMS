using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using MAMS.Models.ViewModels;
using MAMS.Models;

namespace MAMS.Services
{
    public class LoginService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public LoginService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient
            {
                BaseAddress = new Uri(_apiUrl)

            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(bool, string)> LoginAsync(LoginViewModel user)
        {

            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("Login/login", user);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);

                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }


        public async Task<(bool, string)> UserAsync(LoginViewModel user)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("Login/login", user);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<(bool, string)> DoctorRegister(SignUpViewModel model)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("Login/DoctorReg", model);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();


                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<(bool, string)> UserRegister(SignUpViewModel model)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("Login/UserReg", model);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return (false, $"An error occurred: {ex.Message}");
            }
        }
    }
}

