using Microsoft.AspNetCore.Mvc;
//using CaptonseProject.Models;
using web_api_base.ViewModel;
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

        [HttpGet("GetProfilePatient")]
        public async Task<ActionResult> GetProfilePatient(int id)
        {
            // Simulate some data retrieval logic
            var patients = await _patientService.GetProfilePatientById(id);
            if (patients == null)
            {
                return NotFound("Patient not found");
            }
            return Ok(patients);
        }

        [HttpPut("UpdateProfilePatient")]
        public async Task<ActionResult> UpdateProfilePatient(int id, [FromForm] UpdateProfilePatientVM patient, IFormFile file)
        {
            var result = await _patientService.UpdatePatientById(id, patient, file);
            if (result == null)
            {
                return NotFound("Patient not found");
            }
            return Ok(result);
        }

      
    }
}