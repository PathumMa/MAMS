using MAMS.API.Data;
using MAMS.API.DTOs;
using MAMS.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        public readonly ApiDataContext _dbContext;

        public DoctorController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        [HttpGet("availabilities")]
        public async Task<IEnumerable<DoctorAvailableDetails>> GetAllAvailabilityAsync()
        {
            return await _dbContext.DoctorAvailableDetails.ToListAsync();
        }

        [HttpGet("availability/{id}")]
        public async Task<IActionResult> GetAvailablility(int id)
        {
            var doctorAvailabilities = _dbContext.DoctorAvailableDetails.Where(u => u.DoctorId == id)
                .Select(d => new DoctorAvailabilityDto
                {
                    Available_Day = d.Available_Day,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                }).ToList();

            if (doctorAvailabilities.Count == 0)
            {
                return NotFound("Not Any Available Days!");
            }
            else
            {
                return Ok(doctorAvailabilities);
            }
            
        }
    }
}
