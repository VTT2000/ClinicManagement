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
    public class WorkScheduleController : ControllerBase
    {
        private readonly IWorkScheduleService _workScheduleService;
        public WorkScheduleController(IWorkScheduleService workScheduleService)
        {
            _workScheduleService = workScheduleService;
        }

        // role receptionist
        [HttpGet("GetAllWorkScheduleDortorAsync")]
        public async Task<IActionResult> GetAllWorkScheduleDortorAsync()
        {
            var result = await _workScheduleService.GetAllWorkScheduleDortorAsync();
            return Ok(result);
        }

        // role receptionist
        [HttpGet("GetWorkScheduleDortorAsync/{id}")]
        public async Task<IActionResult> GetWorkScheduleDortorAsync([FromRoute]int id)
        {
            var result = await _workScheduleService.GetWorkScheduleDortorAsync(id);
            return Ok(result);
        }

        // role receptionist
        [HttpPost("SaveWorkScheduleDortorAsync")]
        public async Task<IActionResult> SaveWorkScheduleDortorAsync([FromBody]WorkScheduleDoctorDetailVM item)
        {
            var result = await _workScheduleService.SaveWorkScheduleDortorAsync(item);
            return Ok(result);
        }

        // role receptionist
        [HttpDelete("DeleteWorkScheduleDortorAsync/{id}")]
        public async Task<IActionResult> DeleteWorkScheduleDortorAsync([FromRoute]int id)
        {
            var result = await _workScheduleService.DeleteWorkScheduleDortorAsync(id);
            return Ok(result);
        }
   }
}