using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Models.ClinicManagement;
using web_api_base.Models.ViewModel;
//using CaptonseProject.Models;

namespace CaptonseProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        [HttpGet("GetAllUser")]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {
            // Simulate fetching data from a database or service
            var users = await _userService.GetAllUserAsync();
            return Ok(users);
        }
        [HttpPost("Login")]
        public async Task<HTTPResponseClient<UserLoginResultVM>> Login([FromBody] UserLoginVM userLogin)
        {
       
            var result = await _userService.Login(userLogin);
           
            return result;
        }

        [HttpPost("Register")]
        public async Task<HTTPResponseClient<dynamic>> Register([FromBody] UserRegisterVM newUser)
        {

            var result = await _userService.Register(newUser);
            return result;
        }


           
        [HttpGet("GetAllStaff")]
        public async Task<ActionResult<HTTPResponseClient<IEnumerable<User>>>> GetAllStaff()
        {
            var result = await _userService.GetAllUserIsStaffAsync();
            return result;
        }

        [HttpGet("GetAllPatient")]
        public async Task<ActionResult<HTTPResponseClient<IEnumerable<User>>>> GetAllPatient()
        {
            var result = await _userService.GetAllUserIsPatientAsync();
            return result;
        }
        

        [Authorize]
        [HttpGet("/user/GetProfile")]
        public async Task<ActionResult> GetProfile([FromHeader] string authorization)
        {
            var user = await _userService.GetProfileUser(authorization);
            Console.WriteLine("User profile: " + user);
            return Ok(user);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordVM model, [FromHeader] string authorization)
        {
            if (model == null)
            {
                return BadRequest("Invalid password data.");
            }

            var result = await _userService.ChangePassword(model, authorization);
            if (result == null)
            {
                return BadRequest("Failed to change password.");
            }
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Admin)]
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }


    }
}