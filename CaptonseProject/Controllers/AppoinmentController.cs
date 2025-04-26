using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using CaptonseProject.Models;

namespace CaptonseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppoinmentController : ControllerBase
    {
        private readonly IAppointmentService _appoinmentService;
        public AppoinmentController(IAppointmentService appoinmentService)
        {
            _appoinmentService = appoinmentService;
        }

        [HttpGet("GetAllAppointmentPatientAsync")]
        public async Task<IActionResult> GetAllAppointmentPatientAsync()
        {
            var result = await _appoinmentService.GetAllAppointmentPatientAsync();
            return Ok(result);
        }

        [HttpGet("GetAllAppointmentPatientAsync/{date}")]
        public async Task<IActionResult> GetAllAppointmentPatientAsync([FromRoute]string date)
        {
            var result = await _appoinmentService.GetAllAppointmentPatientForDateAsync(date);
            return Ok(result);
        }

        
    }
}