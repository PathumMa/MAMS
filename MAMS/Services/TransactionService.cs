using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MAMS.Models;
using Newtonsoft.Json;
using MAMS.Models.ViewModels;

namespace MAMS.Services
{
    public class TransactionService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public TransactionService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient
            {
                BaseAddress = new Uri(_apiUrl)

            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<TransactionReceiptViewModel?> GetReceiptByRefNoAsync(string refNo)
        {
            var response = await _client.GetAsync($"Transaction/GetReceiptByRef/{refNo}");
            if (!response.IsSuccessStatusCode) 
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TransactionReceiptViewModel>(json);
        }
        public async Task<byte[]> DownloadReceiptPdfAsync(string refNo)
        {
            var response = await _client.GetAsync($"Transaction/DownloadReceiptPdf/{refNo}");
            return await response.Content.ReadAsByteArrayAsync();
        }

    }
}
