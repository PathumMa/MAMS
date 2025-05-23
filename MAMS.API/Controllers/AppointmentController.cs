using MAMS.API.Data;
using MAMS.API.DTOs;
using MAMS.API.Models;
using MAMS.API.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        public readonly ApiDataContext _dbContext;

        public AppointmentController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        [HttpGet("count/week")]
        public async Task<IActionResult> GetWeeklyAppointmentCount([FromQuery] DateTime weekStartDate)
        {
            // Calculate the end date of the week
            var weekEndDate = weekStartDate.AddDays(6);

            // Get the count of appointments for the given week
            var appointmentCount = await _dbContext.Appointments
                .Where(a => a.Appointment_Date.Date >= weekStartDate.Date
                         && a.Appointment_Date.Date <= weekEndDate.Date)
                .CountAsync();

            return Ok(new { WeekStartDate = weekStartDate, WeekEndDate = weekEndDate, AppointmentCount = appointmentCount });
        }

        [HttpPost("Booking")]
        public async Task<IActionResult> PostNewBooking([FromBody] BookingDto newBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_dbContext.Appointments.Any(a => a.Appointment_Date  == newBooking.Appointment_Date && a.Appoinment_number == newBooking.Appoinment_number))
                {
                    return BadRequest("Appointment Number Exist!");
                }
                else if(_dbContext.Appointments.Any(a => a.Appointment_Date == newBooking.Appointment_Date && a.User_PersonalId == newBooking.Personal_Id))
                {
                    return BadRequest("Patient already have an appointment!");
                }

                var appointmentEntity = new Appointments
                {
                    User_PersonalId = newBooking.Personal_Id,
                    Doctor_Id = newBooking.Doctor_Id,
                    Appointment_Date = newBooking.Appointment_Date,
                    Appoinment_number = newBooking.Appoinment_number,
                    Status = newBooking.Status,
                };

                var patientDetails = new PatientDetails
                {
                    //RegisteredUserId = newBooking.RegisteredUserId,
                    UserTitle = newBooking.UserTitle,
                    Name = newBooking.Name,
                    Address = newBooking.Address,
                    City = newBooking.City,
                    BirthDate = newBooking.BirthDate,
                    PersonalIdType = newBooking.PersonalId_Type,
                    PersonalId = newBooking.Personal_Id

                };

                var transactionEntity = new Transactions
                {
                    Doctor_fee = newBooking.Doctor_fee,
                    Hospital_fee = newBooking.Hospital_fee,
                    Discount = newBooking.Discount,
                    Amount = newBooking.Amount,
                    PaymentMethod = newBooking.PaymentMethod
                };

                appointmentEntity.PatientDetails = patientDetails;

                appointmentEntity.Transactions = transactionEntity;

                _dbContext.Appointments.Add(appointmentEntity);

                _dbContext.PatientDetails.Add(patientDetails);

                _dbContext.Transactions.Add(transactionEntity);

                await _dbContext.SaveChangesAsync();

                return Ok($"{newBooking.Appoinment_number} - Booking Success.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("AppoinmentCount")]
        public async Task<IActionResult> GetAppointmentCount(int doctorId, DateTime date)
        {
            var appointmentCount = await _dbContext.Appointments.CountAsync(a => a.Doctor_Id == doctorId && a.Appointment_Date.Date == date);

            if (appointmentCount > 0)
            {
                return Ok(appointmentCount);
            }
            return NotFound("No Appointments");
        }

        [HttpGet("LastNumber")]
        public async Task<IActionResult> GetLastAppointmentNumber(int doctorId, DateTime date)
        {
            var lastNumber = await _dbContext.Appointments
                .Where(a => a.Appointment_Date == date && a.Doctor_Id == doctorId)
                .MaxAsync(a => (int?)a.Appoinment_number);
            if (lastNumber > 0)
            {
                return Ok(lastNumber);
            }
            return NotFound("No Appointments");
        }
    }
}