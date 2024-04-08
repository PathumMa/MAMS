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
            var user = _dbContext.Susers.Where(u => u.UserName == userName).FirstOrDefault<Suser>();

            if (user == null)
            {
                return NotFound("User Not Found!");
            }

            if (user.RoleId != 30)
            {
                var userDetails = _dbContext.UserDetails.Where(u => u.SuserId == user.Id).FirstOrDefault<UserDetails>();
                var userDet = new UserDetailsViewModel
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

                if (userDetails == null)
                {
                    return NotFound("User Details Not Found!");
                }
                else
                {
                    return Ok(userDet);
                }
            }

            else
            {

                var docDetails = _dbContext.DoctorDetails.Where(x => x.SuserId == user.Id).FirstOrDefault<DoctorDetails>();
                var docAvailDetails = _dbContext.DoctorAvailableDetails.Where(y => y.DoctorId == docDetails.Id).FirstOrDefault<DoctorAvailableDetails>();

                var doctorDet = new DoctorDetailsViewModel
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
                    MedicalCouncilRegistrationNumber = docDetails?.MedicalCouncilRegistrationNumber,
                    Specialization = docDetails?.Specialization,
                    Available_Day = docAvailDetails?.Available_Day,
                    Available_Time = docAvailDetails?.Available_Time

                };

                if (docDetails == null)
                {
                    return NotFound("User Details Not Found!");
                }
                else
                {
                    return Ok(doctorDet);
                }
            }

        }

    }
}
