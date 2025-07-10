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
    public class LabBookingController : ControllerBase
    {
        public readonly ApiDataContext _dbContext;
        public LabBookingController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        [HttpGet("availability")]
        public async Task<IActionResult> GetLabAvailability(DateTime date)
        {
            try
            {
                var slotCount = await _dbContext.LabResults
                    .Where(x => x.BookedDate.Date == date.Date)
                    .GroupBy(x => x.TimeSlot)
                    .Select(g => new
                    {
                        TimeSlot = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                return Ok(slotCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("BookLab")]
        public async Task<IActionResult> BookLab([FromBody] LabBookingDto dt)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            // 1. Check if patient exists
            var patient = await _dbContext.PatientDetails
                .FirstOrDefaultAsync(p => p.PersonalId == dt.PersonalId);

            if (patient == null)
            {
                patient = new PatientDetails
                {
                    PersonalId = dt.PersonalId,
                    PersonalIdType = dt.PersonalIdType,
                    UserTitle = dt.UserTitle,
                    Name = dt.Name,
                    PhoneNumber = dt.PhoneNumber,
                    Address = dt.Address,
                    CreatedDate = DateTime.Now
                };

                _dbContext.PatientDetails.Add(patient);
                await _dbContext.SaveChangesAsync();
            }

            // 2. Check booking count for the day
            var bookedCount = await _dbContext.LabResults
                .CountAsync(x => x.BookedDate.Date == dt.BookedDate.Date);

            if (bookedCount >= 50) // Assuming 5 slots * 10 hours max
                return BadRequest("Selected day is fully booked.");

            // 3. Assign time slot (generate logic: 5 patients per hour)
            var nextSlot = GenerateNextAvailableTimeSlot(dt.BookedDate, _dbContext);

            if (nextSlot == null)
                return BadRequest("No available time slots for selected date.");

            // 4. Generate Reference Number
            var todayCount = await _dbContext.LabResults
                .CountAsync(x => x.BookedDate.Date == dt.BookedDate.Date);
            var refNo = $"LAB-{dt.BookedDate:yyyyMMdd}-{(todayCount + 1):D4}";

            var labType = await _dbContext.LabTypes.FindAsync(dt.LabTypeId);

            var labResult = new LabResult
            {
                LabTypeId = dt.LabTypeId,
                PatientId = patient.Id,
                BookedDate = dt.BookedDate.Date,
                TimeSlot = nextSlot,
                Status = AppoinmentStatus.Scheduled,
                BookedPrice = labType.Price,
                ReferenceNo = refNo
            };

            _dbContext.LabResults.Add(labResult);
            await _dbContext.SaveChangesAsync();

            // 5. Save Transaction
            var transaction = new Transactions
            {
                LabResultId = labResult.LabResultId,
                BookingType = BookingType.LabTest,
                Amount = labType.Price,
                PaymentMethod = dt.PaymentMethod,
                Created_Date = DateTime.Now,
                Patient_Id = patient.Id
            };

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();

            var response = new LabBookingResponseDto
            {
                ReferenceNo = refNo,
                Time = nextSlot.ToString(),
                BookedDate = dt.BookedDate.ToString("dd/MM/yyyy"),
                LabName = labType.LabName,
                Price = labType.Price,
                Patient = patient.Name
            };

            return Ok(response);
        }

        private TimeSpan? GenerateNextAvailableTimeSlot(DateTime date, ApiDataContext context)
        {
            var slots = Enumerable.Range(9, 8)  // 9AM to 5PM
                .SelectMany(hour => Enumerable.Range(0, 6)
                    .Select(i => new TimeSpan(hour, i * 10, 0)))
                .ToList(); // 10-minute slots

            foreach (var slot in slots)
            {
                var count = context.LabResults
                    .Count(x => x.BookedDate.Date == date.Date && x.TimeSlot == slot);

                if (count < 10)
                    return slot;
            }

            return null;
        }

    }
}
