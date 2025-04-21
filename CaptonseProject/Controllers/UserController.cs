using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Models.ViewModel;
//using CaptonseProject.Models;

namespace CaptonseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
           var users = await _userService.GetAllAsync();
            return Ok(users);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginVM userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest("Invalid login data.");
            }

            var result = _userService.Login(userLogin);
            if (result == null)
            {
                return Unauthorized("Invalid email or password!.");
            }
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterVM newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data.");
            }

            var result = await _userService.Register(newUser);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Admin)]
        [HttpPost("AdminCreateUser")]
        public async Task<IActionResult> AdminCreateUser([FromBody] AdminRegisterUserVM newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data.");
            }

            var result = await _userService.AdminCreateUser(newUser);
            return Ok(result);
        } 
           

    }
}