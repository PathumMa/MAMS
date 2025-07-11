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
                // Check if patient already booked this doctor on this date
                bool appointmentExists = await _dbContext.Appointments.AnyAsync(a =>
                    a.PatientDetails.PersonalId == newBooking.Personal_Id &&
                    a.Doctor_Id == newBooking.Doctor_Id &&
                    a.Appointment_Date.Date == newBooking.Appointment_Date.Date
                );

                if (appointmentExists)
                {
                    return BadRequest("This patient already has an appointment with this doctor on this date.");
                }

                // Check if appointment number is already used by this doctor on this date
                bool numberExists = await _dbContext.Appointments.AnyAsync(a =>
                    a.Doctor_Id == newBooking.Doctor_Id &&
                    a.Appointment_Date.Date == newBooking.Appointment_Date.Date &&
                    a.Appoinment_number == newBooking.Appoinment_number
                );

                if (numberExists)
                {
                    return BadRequest($"Appointment number {newBooking.Appoinment_number} is already used for this doctor on this date.");
                }

                // Check if patient already exists by PersonalId
                var patientDetails = await _dbContext.PatientDetails
                    .FirstOrDefaultAsync(p => p.PersonalId == newBooking.Personal_Id);

                if (patientDetails == null)
                {
                    patientDetails = new PatientDetails
                    {
                        UserTitle = newBooking.UserTitle,
                        Name = newBooking.Name,
                        PhoneNumber = newBooking.PhoneNumber,
                        Address = newBooking.Address,
                        City = newBooking.City,
                        BirthDate = newBooking.BirthDate,
                        PersonalId = newBooking.Personal_Id,
                        PersonalIdType = newBooking.PersonalId_Type
                    };

                    _dbContext.PatientDetails.Add(patientDetails);
                    await _dbContext.SaveChangesAsync();
                }

                // Create appointment and link to patient
                var appointmentEntity = new Appointments
                {
                    User_PersonalId = newBooking.Personal_Id,
                    Doctor_Id = newBooking.Doctor_Id,
                    Availability_Id = newBooking.Availability_Id,
                    Appointment_Date = newBooking.Appointment_Date,
                    Appoinment_number = newBooking.Appoinment_number,
                    PatientDetails_Id = patientDetails.Id,
                    Status = newBooking.Status
                };

                _dbContext.Appointments.Add(appointmentEntity);
                await _dbContext.SaveChangesAsync();

                // Create transaction and link to appointment and patient
                var transactionEntity = new Transactions
                {
                    Appointment_Id = appointmentEntity.Id,
                    PatientDetails_Id = patientDetails.Id,
                    Doctor_fee = newBooking.Doctor_fee,
                    Hospital_fee = newBooking.Hospital_fee,
                    Discount = newBooking.Discount,
                    Amount = newBooking.Amount,
                    BookingType = BookingType.Doctor,
                    PaymentMethod = newBooking.PaymentMethod
                };

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

        [HttpGet("by-doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentByDoctor(int doctorId, DateTime? date, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbContext.Appointments
                .Include(a => a.PatientDetails)
                .Include(a => a.Transaction)
                .Where(a => a.Doctor_Id == doctorId)
                .AsQueryable();

            // Filter by date or range
            if (date.HasValue)
            {
                query = query.Where(a => a.Appointment_Date.Date == date.Value.Date);
            }
            else
            {
                if (startDate.HasValue)
                    query = query.Where(a => a.Appointment_Date.Date >= startDate.Value.Date);
                if (endDate.HasValue)
                    query = query.Where(a => a.Appointment_Date.Date <= endDate.Value.Date);
            }

            var appointments = await query.ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound($"No appointments found for doctor with ID {doctorId}");
            }

            var result = appointments.Select(a => new AppointmentDetailsDto
            {
                Id = a.Id,
                User_PersonalId = a.User_PersonalId,
                Doctor_Id = a.Doctor_Id,
                Availability_Id = a.Availability_Id,
                Appointment_Date = a.Appointment_Date,
                Appoinment_number = a.Appoinment_number,
                Status = a.Status.ToString(),

                // Patient Info
                PatientName = a.PatientDetails?.Name,
                PatientTitle = a.PatientDetails?.UserTitle,
                PersonalId = a.PatientDetails?.PersonalId,
                PersonalIdType = a.PatientDetails?.PersonalIdType,
                BirthDate = a.PatientDetails?.BirthDate,
                Address = a.PatientDetails?.Address,
                City = a.PatientDetails?.City,

                // Transaction (single wrapped in list)
                Transactions = a.Transaction != null
                    ? new List<TransactionDto>
                    {
                new TransactionDto
                {
                    Doctor_fee = a.Transaction.Doctor_fee,
                    Hospital_fee = a.Transaction.Hospital_fee,
                    Discount = a.Transaction.Discount,
                    Amount = a.Transaction.Amount,
                    PaymentMethod = a.Transaction.PaymentMethod.ToString()
                }
                    }
                    : new List<TransactionDto>()
            });

            return Ok(result);
        }


        [HttpGet("by-patient/{personalId}")]
        public async Task<IActionResult> GetAppointmentByPatient(string personalId, DateTime? date, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbContext.Appointments
                .Include(a => a.PatientDetails)
                .Include(a => a.Transaction)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Specialization)
                .Where(a => a.PatientDetails.PersonalId == personalId)
                .AsQueryable();

            // Date filters
            if (date.HasValue)
            {
                query = query.Where(a => a.Appointment_Date.Date == date.Value.Date);
            }
            else
            {
                if (startDate.HasValue)
                    query = query.Where(a => a.Appointment_Date.Date >= startDate.Value.Date);
                if (endDate.HasValue)
                    query = query.Where(a => a.Appointment_Date.Date <= endDate.Value.Date);
            }

            var appointments = await query.ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound($"No appointments found for patient with Personal ID {personalId}");
            }

            var result = appointments.Select(a => new AppointmentDetailsDto
            {
                Id = a.Id,
                User_PersonalId = a.User_PersonalId,
                Doctor_Id = a.Doctor_Id,
                Availability_Id = a.Availability_Id,
                Appointment_Date = a.Appointment_Date,
                Appoinment_number = a.Appoinment_number,
                Status = a.Status.ToString(),

                // Patient Info
                PatientName = a.PatientDetails?.Name,
                PatientTitle = a.PatientDetails?.UserTitle,
                PersonalId = a.PatientDetails?.PersonalId,
                PersonalIdType = a.PatientDetails?.PersonalIdType,
                BirthDate = a.PatientDetails?.BirthDate,
                Address = a.PatientDetails?.Address,
                City = a.PatientDetails?.City,

                // Doctor Info
                DoctorName = $"{a.Doctor?.First_Name} {a.Doctor?.Last_Name}",
                DoctorSpecialization = a.Doctor?.Specialization?.Specializations_Name,

                // Transaction (only one now)
                Transactions = a.Transaction != null
                    ? new List<TransactionDto>
                    {
                new TransactionDto
                {
                    Doctor_fee = a.Transaction.Doctor_fee,
                    Hospital_fee = a.Transaction.Hospital_fee,
                    Discount = a.Transaction.Discount,
                    Amount = a.Transaction.Amount,
                    PaymentMethod = a.Transaction.PaymentMethod.ToString()
                }
                    }
                    : new List<TransactionDto>()
            });

            return Ok(result);
        }

    }
}