using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;
//using web_api_base.Models;

namespace web_api_base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController(ClinicContext clinicContext) : ControllerBase
    {
      
        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {

            var User = await clinicContext.Users.ToListAsync();    
            return Ok(User);
        }

       

    }
}