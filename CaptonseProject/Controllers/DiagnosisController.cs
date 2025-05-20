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
    public class DiagnosisController : ControllerBase
    {
        private readonly IDiagnosisServiceBE _diagnosisService;
        public DiagnosisController(IDiagnosisServiceBE diagnosisService)
        {
            _diagnosisService = diagnosisService;
        }

        // role doctor
        [Authorize]
        [HttpPost("SaveDiagnosisDoctorAsync")]
        public async Task<IActionResult> SaveDiagnosisDoctorAsync([FromHeader] string authorization, [FromBody] DetailSaveDiagnosisDoctorVM item)
        {
            var result = await _diagnosisService.SaveDiagnosisDoctorAsync(item, authorization);
            return Ok(result);
        }
    

        // role receptionist
        // [HttpPost("GetAllAppointmentPatientAsync")]
        // public async Task<IActionResult> GetAllAppointmentPatientAsync2([FromBody] PagedResponse<ConditionFilterPatientForAppointmentReceptionist> condition)
        // {
        //     var result = await _appointmentService.GetAllAppointmentPatientAsync2(condition);
        //     return Ok(result);
        // }
    }
}