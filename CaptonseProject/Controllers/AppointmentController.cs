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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appoinmentService)
        {
            _appointmentService = appoinmentService;
        }

        // role receptionist
        [HttpGet("GetAllAppointmentPatientAsync")]
        public async Task<IActionResult> GetAllAppointmentPatientAsync()
        {
            var result = await _appointmentService.GetAllAppointmentPatientAsync();
            return Ok(result);
        }

        // role receptionist
        [HttpGet("GetAllAppointmentPatientAsync/{date}")]
        public async Task<IActionResult> GetAllAppointmentPatientAsync([FromRoute]string date)
        {
            var result = await _appointmentService.GetAllAppointmentPatientForDateAsync(date);
            return Ok(result);
        }

        // role receptionist
        [HttpGet("CreateAppointmentFromReceptionist")]
        public async Task<IActionResult> CreateAppointmentFromReceptionist([FromBody]AppointmentReceptionistCreateVM item)
        {
            var result = await _appointmentService.CreateAppointmentFromReceptionist(item);
            return Ok(result);
        }
    }
}