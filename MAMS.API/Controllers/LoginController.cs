using MAMS.API.Data;
using MAMS.API.Models;
using MAMS.API.ViewModels;
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
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginModel)
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

            if (hashedPassword != loginModel.Password)
            {
                return BadRequest("Invalid Password");
            }

            // Authentication successful, you can generate a token or perform other actions here
            return Ok(user.Id);
        }

        [HttpPost("register")]
        public IActionResult SignUp(SignUpViewModel signUpData)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_dbContext.Susers.Any(u => u.UserName == signUpData.Suser.UserName))
                {
                    ModelState.AddModelError("Suser.UserName", "Username is already taken. Please choose a different one.");
                    return BadRequest(ModelState);
                }

                var hashedPassword = Password.HashPassword(signUpData.Suser.Password);

                // Create Suser object
                var newUser = new Suser
                {
                    RoleId = signUpData.Suser.RoleId,
                    UserName = signUpData.Suser.UserName,
                    Password = hashedPassword,
                    Email = signUpData.Suser.Email,
                    PhoneNumber = signUpData.Suser.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    IsActive = 1
                };

                // Create SuserDetails object
                var userDetails = new SuserDetails
                {
                    UserTitle = signUpData.SuserDetails.UserTitle,
                    First_Name = signUpData.SuserDetails.First_Name,
                    Last_Name = signUpData.SuserDetails.Last_Name,
                    Middle_Name = signUpData.SuserDetails.Middle_Name,
                    Address = signUpData.SuserDetails.Address,
                    City = signUpData.SuserDetails.City,
                    District = signUpData.SuserDetails.District,
                    Province = signUpData.SuserDetails.Province,
                    Birth_Date = signUpData.SuserDetails.Birth_Date,
                    Gender = signUpData.SuserDetails.Gender,
                    Blood_Type = signUpData.SuserDetails.Blood_Type,
                    Personal_Id = signUpData.SuserDetails.Personal_Id,
                    PersonalId_Type = signUpData.SuserDetails.PersonalId_Type,
                    CreatedDate = DateTime.Now
                };

                // Create DoctorDetails object (if applicable)
                if (signUpData.Suser.RoleId == 30) // Assuming RoleId 30 is for doctors
                {
                    var doctorAvailableDetails = new DoctorAvailableDetails
                    {
                        AvailableDay = signUpData.DoctorAvailableDetails.AvailableDay,
                        AvailableTime = signUpData.DoctorAvailableDetails.AvailableTime,
                        CreatedDate = DateTime.Now
                    };

                    userDetails.DoctorAvailableDetails = new List<DoctorAvailableDetails> { doctorAvailableDetails };

                    var doctorDetails = new DoctorDetails
                    {
                        Speciality = signUpData.DoctorDetails.Speciality,
                        CreatedDate = DateTime.Now
                    };
                }

                // Assign SuserDetails to Suser
                newUser.SuserDetails = userDetails;

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
     
    }
}