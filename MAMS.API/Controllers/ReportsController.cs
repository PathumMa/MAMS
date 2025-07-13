using MAMS.API.Data;
using MAMS.API.DTOs;
using MAMS.API.Models;
using MAMS.API.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using static MAMS.API.Tools.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        public readonly ApiDataContext _dbContext;
        public ReportsController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        [HttpGet("GetDailyLabReport/{date}")]
        public async Task<IActionResult> GetDailyLabReport(DateTime date)
        {
            try
            {
                var result = await _dbContext.LabBookingSummaryView
                    .Where(x => x.BookedDate.Date == date.Date)
                    .ToListAsync();

                if (result == null || !result.Any())
                {
                    return NotFound($"No lab bookings found for the date {date:yyyy-MM-dd}.");
                }

                    return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetLabReportByRange")]
        public async Task<IActionResult> GetLabReportByDaily(DateTime startdate, DateTime endDate)
        {
            try {
                var result = await _dbContext.LabBookingSummaryView
                        .Where(x => x.BookedDate.Date >= startdate.Date && x.BookedDate.Date <= endDate.Date)
                        .ToListAsync();

                if (result == null || !result.Any())
                {
                    return NotFound($"No lab bookings found for the between {startdate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetDoctorAppointmentSummary/{date}")]
        public async Task<IActionResult> GetDoctorAppointmentSummary(DateTime date)
        {
            try
            {
                var result = await _dbContext.DoctorAppointmentSummaryView
                .Where(x => x.AppointmentDate.Date == date.Date)
                .ToListAsync();

                if(result == null || !result.Any())
                {
                    return NotFound($"No doctor appointments found for the date {date:yyyy-MM-dd}.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }

        }

        [HttpGet("GetRevenueSummary/{date}")]
        public async Task<IActionResult> GetRevenueSummary(DateTime date)
        {
            try
            {
                var result = await _dbContext.Set<RevenueSummaryViewModel>()
                    .FromSqlRaw("SELECT * FROM vw_RevenueSummary WHERE RevenueDate = {0}", date)
                    .ToListAsync();

                if (!result.Any())
                    return NotFound("No revenue data found for the selected date.");

                return Ok(result.First()); // Only one row per date
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
