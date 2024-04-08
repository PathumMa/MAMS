using MAMS.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MAMS.Services
{
    public class UserService
    {
        private readonly string _apiUrl;

        public UserService(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<(UserDetailsViewModel, string)> GetUserDetailsAsync(string user)
        {
            UserDetailsViewModel? viewModel = new UserDetailsViewModel();
            string? errorMessage = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.GetAsync("User/" + user);

                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        viewModel = JsonConvert.DeserializeObject<UserDetailsViewModel>(results);
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

            return (viewModel, errorMessage);
        }
    }
}
