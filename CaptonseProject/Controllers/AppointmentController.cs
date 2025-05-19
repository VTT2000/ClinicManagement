using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("GetAllAppointmentPatientAsync")]
        public async Task<IActionResult> GetAllAppointmentPatientAsync2([FromBody] PagedResponse<ConditionFilterPatientForAppointmentReceptionist> condition)
        {
            var result = await _appointmentService.GetAllAppointmentPatientAsync2(condition);
            return Ok(result);
        }

        // role receptionist
        [HttpPost("CreateAppointmentFromReceptionist")]
        public async Task<IActionResult> CreateAppointmentFromReceptionist([FromBody] AppointmentReceptionistCreateVM item)
        {
            var result = await _appointmentService.CreateAppointmentFromReceptionist(item);
            return Ok(result);
        }

        // role receptionist
        [HttpPost("ChangeStatusWaitingForPatient")]
        public async Task<IActionResult> ChangeStatusWaitingForPatient([FromBody]int appointmentId)
        {
            var result = await _appointmentService.ChangeStatusWaitingForPatient(appointmentId);
            return Ok(result);
        }

        // role doctor
        [HttpGet("GetAllListPatientForDocTor/{date}")]
        public async Task<IActionResult> GetAllListPatientForDocTor([FromRoute] DateOnly date)
        {
            var result = await _appointmentService.GetAllListPatientForDocTor(date);
            return Ok(result);
        }

        // role doctor
        [Authorize]
        [HttpPost("GetAllListPatientForDocTorAsync2")]
        public async Task<IActionResult> GetAllListPatientForDocTorAsync2([FromHeader] string authorization, [FromBody] PagedResponse<ConditionFilterPatientForAppointmentDoctor> condition)
        {
            var result = await _appointmentService.GetAllListPatientForDocTorAsync2(condition, authorization);
            return Ok(result);
        }

        // role doctor
        [HttpPut("UpdateStatusAppointmentForDoctor/{appointmentId}")]
        public async Task<IActionResult> UpdateStatusAppointmentForDoctor([FromRoute] int appointmentId, [FromBody] string status)
        {
            var result = await _appointmentService.UpdateStatusAppointmentForDoctor(appointmentId, status);
            return Ok(result);
        }
        
        // role doctor
        [HttpGet("GetAllFreeTimeAppointmentForDoctor/{date}/{doctorId}")]
        public async Task<IActionResult> GetAllFreeTimeAppointmentForDoctor([FromRoute] DateOnly date,[FromRoute] int doctorId)
        {
            var result = await _appointmentService.GetAllFreeTimeAppointmentForDoctor(date, doctorId);
            return Ok(result);
        }
    }
}