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
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // role receptionist
        [HttpGet("GetByNameForReceptionistAsync/{searchKey}")]
        public async Task<IActionResult> GetByNameForReceptionistAsync([FromRoute]string searchKey)
        {
            var result = await _patientService.GetByNameForReceptionistAsync(searchKey);
            return Ok(result);
        }

   }
}