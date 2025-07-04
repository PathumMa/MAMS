using MAMS.API.Data;
using MAMS.API.DTOs;
using MAMS.API.Models;
using MAMS.API.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : ControllerBase
    {
        public readonly ApiDataContext _dbContext;
        public LabController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        [HttpGet("allTests")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tests = await _dbContext.LabTests
                    .Include(x => x.LabTestLabTestCategories)
                    .ThenInclude(x => x.LabTestCategory)
                    .ToListAsync();

                return Ok(tests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var test = await _dbContext.LabTests
                    .Include(x => x.LabTestLabTestCategories)
                        .ThenInclude(x => x.LabTestCategory)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (test == null)
                    return NotFound();

                return Ok(test);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LabTest test)
        {
            try
            {
                test.CreatedDate = DateTime.Now;

                _dbContext.LabTests.Add(test);
                await _dbContext.SaveChangesAsync();

                return Ok(test);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LabTest updated)
        {
            try
            {
                var test = await _dbContext.LabTests.FindAsync(id);
                if (test == null) return NotFound();

                test.Name = updated.Name;
                test.Description = updated.Description;
                test.Price = updated.Price;
                test.IsActive = updated.IsActive;
                test.UpdatedDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                return Ok(test);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var test = await _dbContext.LabTests.FindAsync(id);
                if (test == null) return NotFound();

                _dbContext.LabTests.Remove(test);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // ─────────────── Lab Category Maintain ───────────────

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _dbContext.LabTestCategories
                    .Include(c => c.LabTestLabTestCategories)
                        .ThenInclude(lc => lc.LabTest)
                    .ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _dbContext.LabTestCategories
                    .Include(c => c.LabTestLabTestCategories)
                        .ThenInclude(lc => lc.LabTest)
                    .FirstOrDefaultAsync(c => c.LabTestCategoryId == id);

                if (category == null) return NotFound();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] LabTestCategory category)
        {
            try
            {
                _dbContext.LabTestCategories.Add(category);
                await _dbContext.SaveChangesAsync();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] LabTestCategory updated)
        {
            try
            {
                var category = await _dbContext.LabTestCategories.FindAsync(id);
                if (category == null) return NotFound();

                category.CategoryName = updated.CategoryName;
                category.Description = updated.Description;

                await _dbContext.SaveChangesAsync();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _dbContext.LabTestCategories.FindAsync(id);
                if (category == null) return NotFound();

                _dbContext.LabTestCategories.Remove(category);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
