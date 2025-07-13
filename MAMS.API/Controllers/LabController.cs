using MAMS.API.Data;
using MAMS.API.DTOs;
using MAMS.API.Models;
using MAMS.API.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MAMS.API.Tools.Enums;

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

        [HttpGet("allTypes")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var types = await _dbContext.LabTypes
            .Include(t => t.LabCategory) 
            .Select(t => new LabTypeDto
            {
                LabTypeId = t.LabTypeId,
                LabName = t.LabName,
                Description = t.Description,
                Price = t.Price,
                LabCategoryId = t.LabCategoryId,
                CategoryName = t.LabCategory.CategoryName,
                IsActive = t.IsActive
            })
            .ToListAsync();

                if (types == null || !types.Any())
                    return NotFound("No lab types found.");

                return Ok(types);

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
                var type = await _dbContext.LabTypes
            .Include(x => x.LabCategory)
            .Where(x => x.LabTypeId == id)
            .Select(x => new LabTypeDto
            {
                LabTypeId = x.LabTypeId,
                LabName = x.LabName,
                Description = x.Description,
                Price = x.Price,
                LabCategoryId = x.LabCategoryId,
                CategoryName = x.LabCategory.CategoryName,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();

                if (type == null)
                    return NotFound();

                return Ok(type);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetBySearch(string? labName, int? categoryId)
        {
            var labTypes = _dbContext.LabTypes
                .Include(x => x.LabCategory)
                .Select(t => new LabTypeDto
                {
                    LabTypeId = t.LabTypeId,
                    LabName = t.LabName,
                    Description = t.Description,
                    Price = t.Price,
                    LabCategoryId = t.LabCategoryId,
                    CategoryName = t.LabCategory.CategoryName,
                    IsActive = t.IsActive
                })
                .AsQueryable();

            try
            {
                if (!string.IsNullOrEmpty(labName))
                {
                    labTypes = labTypes.Where(l => l.LabName.Contains(labName) || l.Description.Contains(labName) || l.CategoryName.Contains(labName));
                }
                else if (categoryId.HasValue)
                {
                    labTypes = labTypes.Where(l => l.LabCategoryId == categoryId.Value);
                }
                else
                {
                    return BadRequest("Lab Name or Category required!");
                }

                var result = await labTypes.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("addType")]
        public async Task<IActionResult> Create([FromBody] LabTypeDto newTypeDto)
        {
            try
            {
                var newType = new LabType
                {
                    LabName = newTypeDto.LabName,
                    Description = newTypeDto.Description,
                    Price = newTypeDto.Price,
                    LabCategoryId = newTypeDto.LabCategoryId
                };

                _dbContext.LabTypes.Add(newType);
                await _dbContext.SaveChangesAsync();

                return Ok(newType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LabTypeDto updatedDto)
        {
            try
            {
                var currentLab = await _dbContext.LabTypes.FindAsync(id);
                if (currentLab == null) return NotFound();

                currentLab.LabName = updatedDto.LabName;
                currentLab.Description = updatedDto.Description;
                currentLab.Price = updatedDto.Price;
                currentLab.LabCategoryId = updatedDto.LabCategoryId;
                currentLab.IsActive = updatedDto.IsActive;
                currentLab.Modified_Date = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                return Ok(currentLab);
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
                var labType = await _dbContext.LabTypes
            .Include(l => l.LabResults)
            .FirstOrDefaultAsync(l => l.LabTypeId == id);

                if (labType == null)
                    return NotFound();

                if (labType.LabResults.Any())
                {
                    // Soft delete: Mark as inactive
                    labType.IsActive = ActiveStatus.Inactive;
                    labType.Modified_Date = DateTime.Now;

                    await _dbContext.SaveChangesAsync();
                    return Accepted("Lab type has bookings and was inactive instead of deleted.");
                }

                // If no bookings, safe to delete
                _dbContext.LabTypes.Remove(labType);
                await _dbContext.SaveChangesAsync();
                return Ok("Lab type deleted successfully.");
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
                var categories = await _dbContext.LabCategories
                    .Select(c => new
                    {
                        c.LabCategoryId,
                        c.CategoryName,
                        c.Description,
                        LabCount = c.LabTypes.Count
                    })
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
                var category = await _dbContext.LabCategories
                    .Include(c => c.LabTypes)
                    .Select(c => new
                    {
                        c.LabCategoryId,
                        c.CategoryName,
                        c.Description,
                        Labs = c.LabTypes.Select(l => new
                        {
                            l.LabTypeId,
                            l.LabName,
                            l.Price,
                            l.IsActive
                        })
                    })
                    .FirstOrDefaultAsync(c => c.LabCategoryId == id);

                if (category == null) return NotFound();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("addCategories")]
        public async Task<IActionResult> CreateCategory([FromBody] LabCategoryDto dto)
        {
            try
            {
                var category = new LabCategory
                {
                    CategoryName = dto.CategoryName,
                    Description = dto.Description
                };

                _dbContext.LabCategories.Add(category);
                await _dbContext.SaveChangesAsync();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("updateCategories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] LabCategoryDto dto)
        {
            try
            {
                var category = await _dbContext.LabCategories.FindAsync(id);
                if (category == null) return NotFound();

                category.CategoryName = dto.CategoryName;
                category.Description = dto.Description;

                await _dbContext.SaveChangesAsync();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("deleteCategories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _dbContext.LabCategories
                    .Include(c => c.LabTypes)
                    .FirstOrDefaultAsync(c => c.LabCategoryId == id);

                if (category == null) return NotFound();

                if (category.LabTypes.Any())
                    return BadRequest("Cannot delete category that contains lab types. Please reassign or remove labs first.");

                _dbContext.LabCategories.Remove(category);
                await _dbContext.SaveChangesAsync();

                return Ok("Category deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
