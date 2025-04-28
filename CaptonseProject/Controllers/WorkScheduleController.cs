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

        [HttpGet("GetAllWorkScheduleDortorAsync")]
        public async Task<IActionResult> GetAllWorkScheduleDortorAsync()
        {
            var result = await _workScheduleService.GetAllWorkScheduleDortorAsync();
            return Ok(result);
        }
   }
}