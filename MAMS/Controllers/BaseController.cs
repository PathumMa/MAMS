using AspNetCoreHero.ToastNotification.Abstractions;
using MAMS.Services;
using MAMS.Services.Reports;
using Microsoft.AspNetCore.Mvc;

namespace MAMS.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly INotyfService _notfy;
        protected readonly IConfiguration _config;
        protected SpecializationService _specializationService;
        protected LoginService _loginService;
        protected UserService _userService;
        protected AvailabilityService _availabilityService;
        protected AppointmentService _appointmentService;
        protected CalanderService _calanderService;
        protected LabService _labService;
        protected TransactionService _transactionService;
        protected ReportService _reportService;

        public BaseController(IConfiguration config, INotyfService notfy, IHttpContextAccessor contextAccessor, AppSettings appSettings)
        {
            _config = config;
            _notfy = notfy;
            _httpContextAccessor = contextAccessor;
            _specializationService = new SpecializationService(appSettings.ApiUrl);
            _loginService = new LoginService(appSettings.ApiUrl);
            _userService = new UserService(appSettings.ApiUrl);
            _availabilityService = new AvailabilityService(appSettings.ApiUrl);
            _appointmentService = new AppointmentService(appSettings.ApiUrl);
            _calanderService = new CalanderService(appSettings.ApiUrl);
            _labService = new LabService(appSettings.ApiUrl);
            _transactionService = new TransactionService(appSettings.ApiUrl);
            _reportService = new ReportService(appSettings.ApiUrl);
        }

        protected bool IsSessionValid()
        {
            string user = _httpContextAccessor.HttpContext.Session.GetString("UserName");

            if (user == null)
            {
                _notfy.Warning("Session Timeout!:", 5);
                return false;
            }

            return true;
        }
    }
}
