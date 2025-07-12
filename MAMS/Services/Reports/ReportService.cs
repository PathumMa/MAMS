using MAMS.Models.ViewModels.Reports;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;


namespace MAMS.Services.Reports
{
    public class ReportService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public ReportService(string apiUrl)
        {
            _apiUrl = apiUrl;

            _client = new HttpClient
            {
                BaseAddress = new Uri(_apiUrl)

            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<LabBookingSummaryViewModel>> GetLabReportAsync(DateTime date)
        {
            var response = await _client.GetAsync($"Reports/GetDailyLabReport/{date:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode)
                return new List<LabBookingSummaryViewModel>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<LabBookingSummaryViewModel>>(json);
        }

        public async Task<List<DoctorAppointmentSummaryViewModel>> GetDoctorAppointmentSummaryAsync(DateTime date)
        {
            var response = await _client.GetAsync($"Reports/GetDoctorAppointmentSummary/{date:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode)
                return new List<DoctorAppointmentSummaryViewModel>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoctorAppointmentSummaryViewModel>>(json);
        }

    }
}
