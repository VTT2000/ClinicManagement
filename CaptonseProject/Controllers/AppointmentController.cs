using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
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

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpPost("GetAllAppointmentPatientAsync")]
        public async Task<IActionResult> GetAllAppointmentPatientAsync2([FromBody] PagedResponse<ConditionFilterPatientForAppointmentReceptionist> condition)
        {
            var result = await _appointmentService.GetAllAppointmentPatientAsync2(condition);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpPost("CreateAppointmentFromReceptionist")]
        public async Task<IActionResult> CreateAppointmentFromReceptionist([FromBody] AppointmentReceptionistCreateVM item)
        {
            var result = await _appointmentService.CreateAppointmentFromReceptionist(item);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpPost("ChangeStatusWaitingForPatient")]
        public async Task<IActionResult> ChangeStatusWaitingForPatient([FromBody]int appointmentId)
        {
            var result = await _appointmentService.ChangeStatusWaitingForPatient(appointmentId);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetStatusAppointmentForDoctorAsync")]
        public async Task<IActionResult> GetStatusAppointmentForDoctorAsync([FromBody] int appointmentID)
        {
            var result = await _appointmentService.GetStatusAppointmentForDoctorAsync(appointmentID);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("IsChangeStatusAppointmentToDiagnosedAsync")]
        public async Task<IActionResult> IsChangeStatusAppointmentToDiagnosedAsync([FromBody] int appointmentID)
        {
            var result = await _appointmentService.IsChangeStatusAppointmentToDiagnosedAsync(appointmentID);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetAllListPatientForDocTorAsync2")]
        public async Task<IActionResult> GetAllListPatientForDocTorAsync2([FromHeader] string authorization, [FromBody] PagedResponse<ConditionFilterPatientForAppointmentDoctor> condition)
        {
            var result = await _appointmentService.GetAllListPatientForDocTorAsync2(condition, authorization);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPut("UpdateStatusAppointmentForDoctor/{appointmentId}")]
        public async Task<IActionResult> UpdateStatusAppointmentForDoctor([FromRoute] int appointmentId, [FromBody] string status)
        {
            var result = await _appointmentService.UpdateStatusAppointmentForDoctor(appointmentId, status);
            return Ok(result);
        }
        
        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpGet("GetAllFreeTimeAppointmentForDoctor/{date}/{doctorId}")]
        public async Task<IActionResult> GetAllFreeTimeAppointmentForDoctor([FromRoute] DateOnly date,[FromRoute] int doctorId)
        {
            var result = await _appointmentService.GetAllFreeTimeAppointmentForDoctor(date, doctorId);
            return Ok(result);
        }
    }
}