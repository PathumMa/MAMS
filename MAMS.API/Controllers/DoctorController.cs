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

        [HttpGet("availability/{doctorId}")]
        public async Task<IActionResult> GetAvailablility(int doctorId)
        {
            var doctorAvailabilities = _dbContext.DoctorAvailableDetails.Where(u => u.DoctorId == doctorId)
                .Select(d => new DoctorAvailabilityDto
                {
                    Id = d.Id,
                    DoctorId = d.DoctorId,
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

        [HttpPut("UpdateAvailability/{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] DoctorAvailableDetails updateAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateAvailability.Id)
            {
                return BadRequest("Invlid Request!. The provided ID in the route does not match the ID in the request body.");
            }

            try
            {
                if (_dbContext.DoctorAvailableDetails.Any(d => d.Id != id && d.Available_Day == updateAvailability.Available_Day))
                {
                    return BadRequest($"{updateAvailability.Available_Day} is already in List!");
                }

                _dbContext.Entry(updateAvailability).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return Ok("Availability Updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.DoctorAvailableDetails.Any(s => s.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
