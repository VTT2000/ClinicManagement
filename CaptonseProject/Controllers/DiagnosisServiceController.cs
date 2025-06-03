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
    public class DiagnosisServiceController : ControllerBase
    {
        private readonly IDiagnosisServiceService _diagnosisServiceService;
        public DiagnosisServiceController(IDiagnosisServiceService diagnosisServiceService)
        {
            _diagnosisServiceService = diagnosisServiceService;
        }

        [Authorize(Roles = RoleConstant.Technician)]
        [HttpPost("GetAllDiagnosisServiceForTechcian")]
        public async Task<IActionResult> GetAllDiagnosisServiceForTechcian([FromBody] PagedResponse<TechnicianConditionFilterParaclinical> condition)
        {
            var result = await _diagnosisServiceService.GetAllDiagnosisServiceForTechcian(condition);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Technician)]
        [HttpGet("GetInfoTestForTechcian/{diagnosisServiceID}")]
        public async Task<IActionResult> GetInfoTestForTechcian([FromRoute] int diagnosisServiceID)
        {
            var result = await _diagnosisServiceService.GetInfoTestForTechcian(diagnosisServiceID);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Technician)]
        [HttpPut("SaveInfoTestForTechcian")]
        public async Task<IActionResult> SaveInfoTestForTechcian([FromHeader] string authorization, [FromBody] TechnicianTestInfoParaclinicalSeviceVM item)
        {
            var result = await _diagnosisServiceService.SaveInfoTestForTechcian(item, authorization);
            return Ok(result);
        }
    }
}