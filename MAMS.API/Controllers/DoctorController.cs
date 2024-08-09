using MAMS.API.Data;
using MAMS.API.DTOs;
using MAMS.API.Models;
using MAMS.API.Models.Views;
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

        [HttpGet("availabilities/{doctorId}")]
        public async Task<IActionResult> GetAvailablilities(int doctorId)
        {
            var doctorAvailabilities = _dbContext.DoctorAvailableDetails.Where(u => u.DoctorId == doctorId)
                .Select(d => new DoctorAvailabilityDto
                {
                    Id = d.Id,
                    DoctorId = d.DoctorId,
                    Available_Day = d.Available_Day,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    ActiveStatus = d.ActiveStatus,
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

        [HttpGet("availability/{availabilityId}")]
        public async Task<ActionResult<DoctorAvailableDetails>> GetAvailability(int availabilityId)
        {
            var availability = _dbContext.DoctorAvailableDetails.Where(a => a.Id == availabilityId).FirstOrDefault<DoctorAvailableDetails>();
            if (availability == null)
            {
                return NotFound("Availability Not Found!");
            }

            return availability;
        }

        [HttpPut("UpdateAvailability/{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] DoctorAvailabilityDto updateAvailability)
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
                var availabilityEntity = await _dbContext.DoctorAvailableDetails.FindAsync(id);
                if (availabilityEntity == null)
                {
                    return NotFound();
                }

                // Update the entity's properties with values from the DTO
                availabilityEntity.DoctorId = updateAvailability.DoctorId;
                availabilityEntity.Available_Day = updateAvailability.Available_Day;
                availabilityEntity.StartTime = updateAvailability.StartTime;
                availabilityEntity.EndTime = updateAvailability.EndTime;
                availabilityEntity.ActiveStatus = updateAvailability.ActiveStatus;

                _dbContext.Entry(availabilityEntity).State = EntityState.Modified;
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

        [HttpPost("AddAvailability")]
        public async Task<IActionResult> AddAvailability([FromBody] DoctorAvailabilityDto addAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (addAvailability.DoctorId == 0)
            {
                return BadRequest("Doctor Id Required.");
            }

            try
            {

                if (_dbContext.DoctorAvailableDetails.Any(d =>
                            d.DoctorId == addAvailability.DoctorId &&
                            d.Available_Day == addAvailability.Available_Day &&
                            (
                                (d.StartTime <= addAvailability.StartTime && addAvailability.StartTime < d.EndTime) ||
                                (d.StartTime < addAvailability.EndTime && addAvailability.EndTime <= d.EndTime) ||
                                (addAvailability.StartTime <= d.StartTime && d.EndTime <= addAvailability.EndTime)
                            )))
                {
                    return BadRequest($"{addAvailability.Available_Day} and {addAvailability.StartTime} is already in List!");
                }

                var availabilityEntity = new DoctorAvailableDetails
                {
                    DoctorId = addAvailability.DoctorId,
                    Available_Day = addAvailability.Available_Day,
                    StartTime = addAvailability.StartTime,
                    EndTime = addAvailability.EndTime,
                    ActiveStatus = addAvailability.ActiveStatus
                };

                _dbContext.DoctorAvailableDetails.Add(availabilityEntity);
                await _dbContext.SaveChangesAsync();

                return Ok("Availability Added Successfully!.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteAvailability/{id}")]
        public async Task<IActionResult> DeleteAvaialbility(int id)
        {
            try
            {
                var availability = await _dbContext.DoctorAvailableDetails.FindAsync(id);

                if (availability == null)
                {
                    return BadRequest("Availability not found that try to Delete!.");
                }

                _dbContext.DoctorAvailableDetails.Remove(availability);
                await _dbContext.SaveChangesAsync();

                return Ok("Specialization deleted successfully.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("DoctorsAll")]
        public async Task<IEnumerable<Doctors>> GetDoctors()
        {
            return await _dbContext.Doctors.ToListAsync();

        }

        [HttpGet("DoctorByName")]
        public async Task<IActionResult> GetDoctorbyName(string? name, string? specialization)
        {
            var doctors = _dbContext.DoctorDetails.AsQueryable();

            if(!string.IsNullOrEmpty(name))
            {
                doctors = doctors.Where(d => d.First_Name.Contains(name) || d.Last_Name.Contains(name) || d.Middle_Name.Contains(name)); 
            }
            else if(!string.IsNullOrEmpty(specialization))
            {
                doctors = doctors.Where(d => d.Specialization.Contains(specialization));
            }
            else
            {
                return BadRequest("Name or Specialization required!");
            }
            
            var result = await doctors.ToListAsync();
            return Ok(result);
        }

    }
}
