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

            // 1. Normalize Personal ID
            dt.PersonalId = dt.PersonalId?.Trim().ToUpper();

            // 2. Check if patient exists
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

            // 3. Check booking count for the day
            var bookedCount = await _dbContext.LabResults
                .CountAsync(x => x.BookedDate.Date == dt.BookedDate.Date);

            if (bookedCount >= 50) // Assuming 5 patients/hour * 10 hours
                return BadRequest("Selected day is fully booked.");

            // 4. Assign next available time slot
            var nextSlot = GenerateNextAvailableTimeSlot(dt.BookedDate, _dbContext);

            if (nextSlot == null)
                return BadRequest("No available time slots for the selected date.");

            // 5. Check if this patient already booked same time slot
            bool alreadyExists = await _dbContext.LabResults
                .AnyAsync(x => x.PatientId == patient.Id
                            && x.BookedDate.Date == dt.BookedDate.Date
                            && x.TimeSlot == nextSlot);

            if (alreadyExists)
                return BadRequest("You have already booked a lab test in this time slot.");

            // 6. Validate lab type
            var labType = await _dbContext.LabTypes.FindAsync(dt.LabTypeId);
            if (labType == null)
                return BadRequest("Invalid Lab Test selected.");

            // 7. Generate Reference Number
            var todayCount = await _dbContext.LabResults
                .CountAsync(x => x.BookedDate.Date == dt.BookedDate.Date);
            var refNo = $"LAB-{dt.BookedDate:yyyyMMdd}-{(todayCount + 1):D4}";

            // 8. Save Lab Result
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

            // 9. Save Transaction
            var transaction = new Transactions
            {
                LabResultId = labResult.LabResultId,
                BookingType = BookingType.LabTest,
                Amount = labType.Price,
                PaymentMethod = dt.PaymentMethod,
                PatientDetails_Id = patient.Id,
                Created_Date = DateTime.Now,
            };
            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();

            // 11. Prepare response
            var response = new LabBookingResponseDto
            {
                ReferenceNo = refNo,
                Time = string.Format("{0:hh\\:mm}", nextSlot), // Format to 09:00
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
