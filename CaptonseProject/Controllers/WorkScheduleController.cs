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
    public class WorkScheduleController : ControllerBase
    {
        private readonly IWorkScheduleService _workScheduleService;
        public WorkScheduleController(IWorkScheduleService workScheduleService)
        {
            _workScheduleService = workScheduleService;
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpGet("GetAllWorkScheduleDortorAsync")]
        public async Task<IActionResult> GetAllWorkScheduleDortorAsync()
        {
            var result = await _workScheduleService.GetAllWorkScheduleDortorAsync();
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpGet("GetWorkScheduleDortorAsync/{id}")]
        public async Task<IActionResult> GetWorkScheduleDortorAsync([FromRoute]int id)
        {
            var result = await _workScheduleService.GetWorkScheduleDortorAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpPost("SaveWorkScheduleDortorAsync")]
        public async Task<IActionResult> SaveWorkScheduleDortorAsync([FromBody]WorkScheduleDoctorDetailVM item)
        {
            var result = await _workScheduleService.SaveWorkScheduleDortorAsync(item);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Receptionist)]
        [HttpDelete("DeleteWorkScheduleDortorAsync/{id}")]
        public async Task<IActionResult> DeleteWorkScheduleDortorAsync([FromRoute]int id)
        {
            var result = await _workScheduleService.DeleteWorkScheduleDortorAsync(id);
            return Ok(result);
        }
   }
}