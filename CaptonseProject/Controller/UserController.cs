using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Models.ClinicManagement;
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
       
    }
}