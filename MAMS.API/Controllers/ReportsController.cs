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

    }
}
