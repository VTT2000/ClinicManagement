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
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetAllMedicineForDoctorByIdAsync")]
        public async Task<IActionResult> GetAllMedicineForDoctorByIdAsync([FromBody] List<int> listID)
        {
            var result = await _medicineService.GetAllMedicineForDoctorByIdAsync(listID);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetAllMedicineForSearchDoctor")]
        public async Task<IActionResult> GetAllMedicineForSearchDoctor([FromBody] PagedResponse<string> pageSearch)
        {
            var result = await _medicineService.GetAllMedicineForSearchDoctor(pageSearch);
            return Ok(result);
        }
    }
}