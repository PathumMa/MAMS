using MAMS.API.Data;
using MAMS.API.Models;
using MAMS.API.DTOs;
using MAMS.API.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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
                    Id = userDetails.Id,
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
                Id = docDetails.Id,
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
                Hospital_Affiliation = docDetails.Hospital_Affiliation,
                Doctor_Fee = docDetails.Doctor_Fee,
                Availabilities = docAvailDetails // Include availability information
            };

            return Ok(doctorDet);
        }

        [HttpGet("doctorDetails/{doctorId}")]
        public IActionResult GetDoctorDetailsByDoctorId(int doctorID)
        {
            // If it's a doctor role
            var docDetails = _dbContext.DoctorDetails.FirstOrDefault(d => d.Id == doctorID);

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
                Id = docDetails.Id,
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
                Hospital_Affiliation = docDetails.Hospital_Affiliation,
                Doctor_Fee = docDetails.Doctor_Fee,
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
                   Hospital_Affiliation = d.Hospital_Affiliation,
                   Doctor_Fee = d.Doctor_Fee
               }).ToListAsync();

            return Ok(doctors);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserDet(string userName, [FromBody] UserDetailsDto updateUser)
        {
            if (userName != updateUser.UserName)
            {
                return BadRequest("Invalid Request!. The provided UserName in the route does not match the UserName in the request body.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingUser = await _dbContext.Susers.Include(u => u.UserDetails).FirstOrDefaultAsync(u => u.UserName == userName);

                if (existingUser == null)
                {
                    return NotFound($"User with UserName '{userName}' not found.");
                }

                if (_dbContext.Susers.Any(s => s.UserName != userName && (s.PhoneNumber == updateUser.PhoneNumber || s.Email == updateUser.Email)))
                {
                    return BadRequest("Email or Phone Number is already in use!");
                }

                existingUser.PhoneNumber = updateUser.PhoneNumber;
                existingUser.Email = updateUser.Email;

                if (existingUser.UserDetails == null)
                {
                    existingUser.UserDetails = new UserDetails();
                }

                existingUser.UserDetails.UserTitle = updateUser.UserName;
                existingUser.UserDetails.First_Name = updateUser.First_Name;
                existingUser.UserDetails.Middle_Name = updateUser.Middle_Name;
                existingUser.UserDetails.Last_Name = updateUser.Last_Name;
                existingUser.UserDetails.Address = updateUser.Address;
                existingUser.UserDetails.City = updateUser.City;
                existingUser.UserDetails.District = updateUser.District;
                existingUser.UserDetails.Province = updateUser.Province;
                existingUser.UserDetails.Birth_Date = updateUser.Birth_Date;
                existingUser.UserDetails.Gender = updateUser.Gender;
                existingUser.UserDetails.Blood_Type = updateUser.Blood_Type;
                existingUser.UserDetails.Personal_Id = updateUser.Personal_Id;
                existingUser.UserDetails.PersonalId_Type = updateUser.PersonalId_Type;
                existingUser.UserDetails.Modified_By = updateUser.Modified_By;
                existingUser.UserDetails.Modified_Date = DateTime.Now;

                _dbContext.Entry(existingUser).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return Ok($"{userName} updated successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("UpdateDoc")]
        public async Task<IActionResult> UpdateDocDet(string userName, [FromBody] DoctorDetailsDto updateDoc)
        {
            if (userName != updateDoc.UserName)
            {
                return BadRequest("Invalid Request!. The provided UserName in the route does not match the UserName in the request body.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingUser = await _dbContext.Susers.Include(u => u.DoctorDetails).FirstOrDefaultAsync(u => u.UserName == userName);

                if (existingUser == null)
                {
                    return NotFound($"User with UserName '{userName}' not found.");
                }

                if (_dbContext.Susers.Any(s => s.UserName != userName && (s.PhoneNumber == updateDoc.PhoneNumber || s.Email == updateDoc.Email)))
                {
                    return BadRequest("Email or Phone Number is already in use!");
                }

                existingUser.PhoneNumber = updateDoc.PhoneNumber;
                existingUser.Email = updateDoc.Email;

                if (existingUser.DoctorDetails == null)
                {
                    existingUser.DoctorDetails = new DoctorDetails();
                }

                existingUser.DoctorDetails.UserTitle = updateDoc.UserTitle;
                existingUser.DoctorDetails.First_Name = updateDoc.First_Name;
                existingUser.DoctorDetails.Middle_Name = updateDoc.Middle_Name;
                existingUser.DoctorDetails.Last_Name = updateDoc.Last_Name;
                existingUser.DoctorDetails.Address= updateDoc.Address;
                existingUser.DoctorDetails.City = updateDoc.City;
                existingUser.DoctorDetails.District = updateDoc.District;
                existingUser.DoctorDetails.Province = updateDoc.Province;
                existingUser.DoctorDetails.Birth_Date = updateDoc.Birth_Date;
                existingUser.DoctorDetails.Gender = updateDoc.Gender;
                existingUser.DoctorDetails.Blood_Type = updateDoc.Blood_Type;
                existingUser.DoctorDetails.Personal_Id = updateDoc.Personal_Id;
                existingUser.DoctorDetails.PersonalId_Type = updateDoc.PersonalId_Type;
                existingUser.DoctorDetails.MedicalCouncilRegistrationNumber = updateDoc.MedicalCouncilRegistrationNumber;
                existingUser.DoctorDetails.Specialization = updateDoc.Specialization;
                existingUser.DoctorDetails.Hospital_Affiliation = updateDoc.Hospital_Affiliation;
                existingUser.DoctorDetails.Doctor_Fee = updateDoc.Doctor_Fee;


                _dbContext.Entry(existingUser).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return Ok($"{userName} updated successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
