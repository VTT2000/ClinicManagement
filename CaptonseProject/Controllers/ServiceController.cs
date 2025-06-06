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
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetServiceVMByIDAsync")]
        public async Task<IActionResult> GetServiceVMByIDAsync([FromBody] int serviceID)
        {
            var result = await _serviceService.GetServiceVMByIDAsync(serviceID);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Technician)]
        [HttpPost("GetServiceVMByIDAsync2")]
        public async Task<IActionResult> GetServiceVMByIDAsync2([FromBody] int serviceID)
        {
            var result = await _serviceService.GetServiceVMByIDAsync2(serviceID);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetAllServiceVMByIDAsync")]
        public async Task<IActionResult> GetAllServiceVMByIDAsync([FromBody] ConditionParaClinicalServiceInfo list)
        {
            var result = await _serviceService.GetAllServiceVMByIDAsync(list);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetAllServiceClinicalAsync")]
        public async Task<IActionResult> GetAllServiceClinicalAsync([FromBody] PagedResponse<string> pagedResponseSearchText)
        {
            var result = await _serviceService.GetAllServiceClinicalAsync(pagedResponseSearchText);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Technician)]
        [HttpPost("GetAllServiceParaclinicalAsync2")]
        public async Task<IActionResult> GetAllServiceParaclinicalAsync2([FromBody] PagedResponse<string> condition)
        {
            var result = await _serviceService.GetAllServiceParaclinicalAsync2(condition);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor)]
        [HttpPost("GetAllServiceParaclinicalAsync")]
        public async Task<IActionResult> GetAllServiceParaclinicalAsync([FromBody] PagedResponse<ConditionFilterParaclinicalServiceSelected> condition)
        {
            var result = await _serviceService.GetAllServiceParaclinicalAsync(condition);
            return Ok(result);
        }
        
    }
}