using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Models.dbebay;
//using web_api_base.Models;

namespace web_api_base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _productService) : ControllerBase
    {
        [HttpGet("GetAllProductListing")]
        public async Task<ActionResult> GetAllProductListing()
        {   
            return Ok(_productService.GetAllProductListing());
        }
    }
}


