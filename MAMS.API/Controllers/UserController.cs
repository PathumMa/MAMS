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


        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailsViewModel>> GetUserDet(int id)
        {
            var user = _dbContext.Susers.Where(u => u.Id == id).FirstOrDefault<Suser>();
            var userDetails = _dbContext.SuserDetails.Where(u => u.SuserId == id).FirstOrDefault<SuserDetails>();
            var docDetails = _dbContext.DoctorDetails.Where(x => x.SuserDetailsId == userDetails.Id).FirstOrDefault<DoctorDetails>();
            var docAvailDetails = _dbContext.DoctorAvailableDetails.Where(y => y.SuserDetailsId == userDetails.Id).FirstOrDefault<DoctorAvailableDetails>();


            var userdet = new UserDetailsViewModel
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
                Personal_Id = userDetails.Personal_Id,
                AvailableDay = docAvailDetails?.AvailableDay,
                AvailableTime = docAvailDetails?.AvailableTime,
                Speciality = docDetails?.Speciality
            };

            if (userDetails == null)
            {
                return NotFound("User Details Not Found!");
            }
            else
            {
                return Ok(userdet);
            }
        }

    }
}
