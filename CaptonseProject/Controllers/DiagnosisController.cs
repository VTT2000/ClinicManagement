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

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("SaveDiagnosisDoctorAsync")]
        public async Task<IActionResult> SaveDiagnosisDoctorAsync([FromHeader] string authorization, [FromBody] DetailSaveDiagnosisDoctorVM item)
        {
            var result = await _diagnosisService.SaveDiagnosisDoctorAsync(item, authorization);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetAllDiagnosisByAppointmentIDAsync")]
        public async Task<IActionResult> GetAllAppointmentPatientAsync2([FromHeader] string authorization, [FromBody] int appointmentID)
        {
            var result = await _diagnosisService.GetAllDiagnosisByAppointmentIDAsync(appointmentID, authorization);
            return Ok(result);
        }
    }
}