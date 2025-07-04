using AspNetCore;
using MAMS.Models;
using MAMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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

        //public async Task<List<LabTestViewModel>> GetAllAsync()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await _client.GetAsync("Lab/allTests");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var results = await response.Content.ReadAsStringAsync();
        //        }
        //    }
        //}
    }
}