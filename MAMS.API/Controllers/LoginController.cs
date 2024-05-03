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
    public class LoginController : Controller
    {
        public readonly ApiDataContext _dbContext;

        public LoginController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Suser>> Index()
        {
            var users = _dbContext.Susers.ToList();

            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var hashedPassword = Password.HashPassword(loginModel.Password);
            var user = await _dbContext.Susers.FirstOrDefaultAsync(u => u.UserName == loginModel.UserName);

            if (user == null)
            {
                return NotFound("Invalid UserName");
            }

            if (hashedPassword != user.Password)
            {
                return BadRequest("Invalid Password");
            }

            // Authentication successful, you can generate a token or perform other actions here
            return Ok(user);
        }

        [HttpPost("UserReg")]
        public IActionResult UserReg(UserSignUpDto signUpData)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_dbContext.Susers.Any(u => u.UserName == signUpData.UserName))
                {
                    ModelState.AddModelError("Suser.UserName", "Username is already taken. Please choose a different one.");
                    return BadRequest(ModelState);
                }

                var hashedPassword = Password.HashPassword(signUpData.Password);

                // Create Suser object
                var newUser = new Suser
                {
                    RoleId = signUpData.RoleId,
                    UserName = signUpData.UserName,
                    Password = hashedPassword,
                    Email = signUpData.Email,
                    PhoneNumber = signUpData.PhoneNumber
                };

                // Create UserDetails object
                var userDetails = new UserDetails
                {
                    UserTitle = signUpData.UserTitle,
                    First_Name = signUpData.First_Name,
                    Last_Name = signUpData.Last_Name,
                    Middle_Name = signUpData.Middle_Name,
                    Address = signUpData.Address,
                    City = signUpData.City,
                    District = signUpData.District,
                    Province = signUpData.Province,
                    Birth_Date = signUpData.Birth_Date,
                    Gender = signUpData.Gender,
                    Blood_Type = signUpData.Blood_Type,
                    Personal_Id = signUpData.Personal_Id,
                    PersonalId_Type = signUpData.PersonalId_Type,
                    Created_Date = DateTime.Now
                };

                // Assign UserDetails to Suser
                newUser.UserDetails = userDetails;

                // Add entities to the database
                _dbContext.Susers.Add(newUser);
                _dbContext.SaveChanges();

                return Ok("User registered successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("DoctorReg")]
        public IActionResult DoctorReg([FromBody] DoctorSignUpDto signUpData)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_dbContext.Susers.Any(u => u.UserName == signUpData.UserName))
                {
                    ModelState.AddModelError("Suser.UserName", "Username is already taken. Please choose a different one.");
                    return BadRequest(ModelState);
                }

                var hashedPassword = Password.HashPassword(signUpData.Password);

                // Create Suser object
                var newUser = new Suser
                {
                    RoleId = signUpData.RoleId,
                    UserName = signUpData.UserName,
                    Password = hashedPassword,
                    Email = signUpData.Email,
                    PhoneNumber = signUpData.PhoneNumber
                };

                // Create DoctorDetails object
                var doctorDetails = new DoctorDetails
                {
                    UserTitle = signUpData.UserTitle,
                    First_Name = signUpData.First_Name,
                    Last_Name = signUpData.Last_Name,
                    Middle_Name = signUpData.Middle_Name,
                    Address = signUpData.Address,
                    City = signUpData.City,
                    District = signUpData.District,
                    Province = signUpData.Province,
                    Birth_Date = signUpData.Birth_Date,
                    Gender = signUpData.Gender,
                    Blood_Type = signUpData.Blood_Type,
                    Personal_Id = signUpData.Personal_Id,
                    PersonalId_Type = signUpData.PersonalId_Type,
                    MedicalCouncilRegistrationNumber = signUpData.MedicalCouncilRegistrationNumber,
                    Specialization = signUpData.Specialization,
                    Hospital_Affiliation = signUpData.Hospital_Affiliation
                };

                // Add available details for the doctor
                //var availabilityDetails = signUpData.Availabilities.Select(a => new DoctorAvailableDetails
                //{
                //    Available_Day = a.Available_Day,
                //    StartTime = a.StartTime ?? TimeSpan.Zero,
                //    EndTime = a.EndTime ?? TimeSpan.Zero,
                //    Created_Date = DateTime.Now
                //}).ToList();

                // Assign doctorDetails to Suser
                newUser.DoctorDetails = doctorDetails;

                // Link the availability to the doctor
                //doctorDetails.AvailableDetails = availabilityDetails;

                // Add entities to the database
                _dbContext.Susers.Add(newUser);
                // Add the new user to the context
                _dbContext.DoctorDetails.Add(doctorDetails);

                _dbContext.SaveChanges();

                return Ok("Doctor registered successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}