using MAMS.API.Data;
using MAMS.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MAMS.API.Tools;

namespace MAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        public readonly ApiDataContext _dbContext;

        public SpecializationController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Specializations>> Get()
        {
            return _dbContext.Specializations.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Specializations>> Get(int id)
        {
            var speciality = _dbContext.Specializations.Where(u => u.Specializations_Id == id).FirstOrDefault<Specializations>();
            if (speciality == null)
            {
                return NotFound("Specialization Not Found!");
            }
            return speciality;
        }

        [HttpPost("addSpec")]
        public async Task<IActionResult> PostSpec([FromBody] Specializations newSpec)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_dbContext.Specializations.Any(u => u.Specializations_Name == newSpec.Specializations_Name))
                {
                    ModelState.AddModelError("specializations.Specializations_Name", "Specialization is already exists!.");
                    return BadRequest(ModelState);
                }

                _dbContext.Specializations.Add(newSpec);
                await _dbContext.SaveChangesAsync();

                return Ok("Specialization Added successfully!.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("updateSpec/{id}")]
        public async Task<IActionResult> UpdateSpec(int id, [FromBody] Specializations updateSpec)
        {
            if (id != updateSpec.Specializations_Id)
            {
                return BadRequest("Invlid Request!. The provided ID in the route does not match the ID in the request body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_dbContext.Specializations.Any(s => s.Specializations_Id != id && s.Specializations_Name == updateSpec.Specializations_Name))
                {
                    ModelState.AddModelError("Specializations_Name", "Specialization name is already in use");
                    return BadRequest(ModelState);
                }

                _dbContext.Entry(updateSpec).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return Ok("Specialization updated successfully.");
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Specializations.Any(s => s.Specializations_Id == id))
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

        [HttpPost("UpdateRecordStatus")]
        public async Task<IActionResult> UpdateRecordStatus(int specializationId, bool isChecked)
        {
            try
            {
                var specialization = await _dbContext.Specializations.FindAsync(specializationId);
                if (specialization != null)
                {
                    specialization.Record_Status = (int)(isChecked ? Enums.ActiveStatus.Active : Enums.ActiveStatus.Inactive);
                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }

                return NotFound();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and return a more specific error message
                return StatusCode(500, $"Database update failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpDelete("deleteSpec/{id}")]

        public async Task<IActionResult> DeleteSpec(int id)
        {
            try
            {
                var specToDelete = await _dbContext.Specializations.FindAsync(id);

                if (specToDelete == null)
                {
                    return NotFound("Specialization not found!.");
                }

                _dbContext.Specializations.Remove(specToDelete);
                await _dbContext.SaveChangesAsync();

                return Ok("Specialization deleted successfully.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}