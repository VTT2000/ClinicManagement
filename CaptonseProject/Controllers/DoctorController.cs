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
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // role receptionist
        [HttpGet("GetByNameForReceptionistAsync/{searchKey}")]
        public async Task<IActionResult> GetByNameForReceptionistAsync([FromRoute]string searchKey)
        {
            var result = await _doctorService.GetByNameForReceptionistAsync(searchKey);
            return Ok(result);
        }

   }
}