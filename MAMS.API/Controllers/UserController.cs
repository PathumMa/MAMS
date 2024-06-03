using MAMS.API.Data;
using MAMS.API.Models;
using MAMS.API.DTOs;
using MAMS.API.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly ApiDataContext _dbContext;

        public UserController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }


        [HttpGet("{userName}")]
        public IActionResult GetUserDet(string userName)
        {
            // Get user by username
            var user = _dbContext.Susers.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            // Check if it's a non-doctor role
            if (user.RoleId != 30)
            {
                var userDetails = _dbContext.UserDetails.FirstOrDefault(u => u.SuserId == user.Id);

                if (userDetails == null)
                {
                    return NotFound("User Details Not Found");
                }

                var userDet = new UserDetailsDto
                {
                    RoleId = user.RoleId,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserTitle = userDetails.UserTitle,
                    First_Name = userDetails.First_Name,
                    Last_Name = userDetails.Last_Name,
                    Middle_Name = userDetails.Middle_Name,
                    Address = userDetails.Address,
                    City = userDetails.City,
                    District = userDetails.District,
                    Province = userDetails.Province,
                    Birth_Date = userDetails.Birth_Date,
                    Blood_Type = userDetails.Blood_Type,
                    Gender = userDetails.Gender,
                    PersonalId_Type = userDetails.PersonalId_Type,
                    Personal_Id = userDetails.Personal_Id
                };

                return Ok(userDet);
            }

            // If it's a doctor role
            var docDetails = _dbContext.DoctorDetails.FirstOrDefault(d => d.SuserId == user.Id);

            if (docDetails == null)
            {
                return NotFound("Doctor Details Not Found");
            }

            // Fetch doctor availability details
            var docAvailDetails = _dbContext.DoctorAvailableDetails
                .Where(d => d.DoctorId == docDetails.Id)
                .Select(ad => new DoctorAvailabilityDto
                {
                    Available_Day = ad.Available_Day,
                    StartTime = ad.StartTime,
                    EndTime = ad.EndTime
                })
                .ToList();

            var doctorDet = new DoctorDetailsDto
            {
                RoleId = user.RoleId,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserTitle = docDetails.UserTitle,
                First_Name = docDetails.First_Name,
                Last_Name = docDetails.Last_Name,
                Middle_Name = docDetails.Middle_Name,
                Address = docDetails.Address,
                City = docDetails.City,
                District = docDetails.District,
                Province = docDetails.Province,
                Birth_Date = docDetails.Birth_Date,
                Blood_Type = docDetails.Blood_Type,
                Gender = docDetails.Gender,
                PersonalId_Type = docDetails.PersonalId_Type,
                Personal_Id = docDetails.Personal_Id,
                MedicalCouncilRegistrationNumber = docDetails.MedicalCouncilRegistrationNumber,
                Specialization = docDetails.Specialization,
                Availabilities = docAvailDetails // Include availability information
            };

            return Ok(doctorDet);
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
             var doctors = await _dbContext.DoctorDetails
                .Include(dd => dd.Suser)
                .Where(dd => dd.Suser.RoleId == 30)
                .Select(d => new DoctorDetailsDto
                {
                    Id = d.Id,
                    RoleId = d.Suser.RoleId,
                    UserName = d.Suser.UserName,
                    Email = d.Suser.Email,
                    PhoneNumber = d.Suser.PhoneNumber,
                    UserTitle = d.UserTitle,
                    First_Name = d.First_Name,
                    Last_Name = d.Last_Name,
                    Middle_Name = d.Middle_Name,
                    Address = d.Address,
                    City = d.City,
                    District = d.District,
                    Province = d.Province,
                    Birth_Date = d.Birth_Date,
                    Gender = d.Gender,
                    Blood_Type = d.Blood_Type,
                    Personal_Id = d.Personal_Id,
                    PersonalId_Type = d.PersonalId_Type,
                    MedicalCouncilRegistrationNumber = d.MedicalCouncilRegistrationNumber,
                    Specialization = d.Specialization,
                    Hospital_Affiliation = d.Hospital_Affiliation
                }).ToListAsync();

            return Ok(doctors);
        }

    }
}
