using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using MAMS.Models.ViewModels;

namespace MAMS.Services
{
    public class LoginService
    {
        private readonly string _apiUrl;

        public LoginService(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<(bool, string)> LoginAsync(LoginViewModel user)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync("Login/login", user);

                    if (getData.IsSuccessStatusCode)
                    {
                        return (true, null);

                    }
                    else
                    {
                        string errorMessage = await getData.Content.ReadAsStringAsync();
                        return (false, errorMessage);
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync("Login/login", user);

                    if (getData.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }
                    else
                    {
                        string errorMessage = await getData.Content.ReadAsStringAsync();
                        return (false, errorMessage);
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync("Login/DoctorReg", model);

                    if (getData.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }
                    else
                    {
                        string errorMessage = await getData.Content.ReadAsStringAsync();
                        return (false, errorMessage);
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync("Login/UserReg", model);

                    if (getData.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }
                    else
                    {
                        string errorMessage = await getData.Content.ReadAsStringAsync();
                        return (false, errorMessage);
                    }
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

