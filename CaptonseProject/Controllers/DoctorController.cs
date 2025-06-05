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
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpGet("GetByNameForReceptionistAsync/{searchKey}")]
        public async Task<IActionResult> GetByNameForReceptionistAsync([FromRoute] string searchKey)
        {
            var result = await _doctorService.GetByNameForReceptionistAsync(searchKey);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpPost("GetAllDoctorForSelectedDoctorAsync")]
        public async Task<IActionResult> GetAllDoctorForSelectedDoctorAsync([FromBody] PagedResponse<ReceptionistConditionFIlterForSelectedDoctor> pagedResponse)
        {
            var result = await _doctorService.GetAllDoctorForSelectedDoctorAsync(pagedResponse);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpGet("GetDoctorByIdAsync/{id}")]
        public async Task<IActionResult> GetDoctorByIdAsync([FromRoute] int id)
        {
            var result = await _doctorService.GetDoctorByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpGet("GetDoctorSelectedByIdAsync/{id}")]
        public async Task<IActionResult> GetDoctorSelectedByIdAsync([FromRoute] int id)
        {
            var result = await _doctorService.GetDoctorSelectedByIdAsync(id);
            return Ok(result);
        }
   }
}