using AspNetCore;
using MAMS.Models;
using MAMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MAMS.Services
{
    public class CalanderService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public CalanderService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient()
            {
                BaseAddress = new Uri(_apiUrl)
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public DateTime GetDateForDayOfWeek(DayOfWeek dayOfWeek)
        {
            DateTime today = DateTime.Today;
            int diff = dayOfWeek - today.DayOfWeek;
            if (diff < 0) // If the day is in the previous week
            {
                diff += 7;
            }
            return today.AddDays(diff);
        }

        public DateTime GetDateForDayOfFutureWeek(DayOfWeek dayOfWeek, int weekOffset = 0)
        {
            DateTime today = DateTime.Today;
            int diff = dayOfWeek - today.DayOfWeek + (weekOffset * 7);
            if (diff < 0)
            {
                diff += 7;
            }
            return today.AddDays(diff);
        }


    }
}
