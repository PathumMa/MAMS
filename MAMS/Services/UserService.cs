using MAMS.Models;
using MAMS.Models.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MAMS.Services
{
    public class UserService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public UserService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient
            {
                BaseAddress = new Uri(_apiUrl)

            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(UserDetailsViewModel, string)> GetUserDetailsAsync(string user)
        {
            UserDetailsViewModel? viewModel = new UserDetailsViewModel();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage getData = await _client.GetAsync("User/" + user);

                    if (getData.IsSuccessStatusCode)
                    {
                        string results = await getData.Content.ReadAsStringAsync();
                        viewModel = JsonConvert.DeserializeObject<UserDetailsViewModel>(results);
                    }
                    else
                    {
                        errorMessage = await getData.Content.ReadAsStringAsync();
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (viewModel, errorMessage);
        }

        public async Task<(DoctorDetailsViewModel, string)> GetDoctorDetailsAsync(string user)
        {
            DoctorDetailsViewModel? viewModel = new DoctorDetailsViewModel();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage getData = await _client.GetAsync("User/" + user);

                if (getData.IsSuccessStatusCode)
                {
                    string results = await getData.Content.ReadAsStringAsync();
                    viewModel = JsonConvert.DeserializeObject<DoctorDetailsViewModel>(results);
                }
                else
                {
                    errorMessage = await getData.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (viewModel, errorMessage);
        }

        public async Task<(IList<DoctorDetailsViewModel>, string?)> GetDoctorsAsync()
        {
            IList<DoctorDetailsViewModel> viewModel = new List<DoctorDetailsViewModel>();
            string? errorMessage = null;

            try
            {
                HttpResponseMessage getData = await _client.GetAsync("User/doctors");

                    if (getData.IsSuccessStatusCode)
                    {
                        string results = await getData.Content.ReadAsStringAsync();
                        viewModel = JsonConvert.DeserializeObject<List<DoctorDetailsViewModel>>(results);
                    }
                    else
                    {
                        errorMessage = await getData.Content.ReadAsStringAsync();
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (viewModel, errorMessage);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateUserDetailsAsync(UserDetails userDetails)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("User/UpdateUser", userDetails);

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
