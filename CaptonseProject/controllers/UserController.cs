// //api-controller
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.OutputCaching;
// using Microsoft.EntityFrameworkCore;
// using web_api_base.Helper;
// using web_api_base.Models.dbebay;
// using web_api_base.Models.ViewModel;
// //using webapi_blazor.Models;

// namespace web_api_base.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class UserController(IUserService _userService) : ControllerBase
//     {
//         // public IUserService _userService;
//         // public UserController(IUserService userService)
//         // {
//         //     _userService = userService;
//         // }

//         [HttpPost("/user/login")]
//         public async Task<ActionResult> Login(UserLoginVM userLogin)
//         {
//             var res =  await _userService.Login(userLogin) as OkObjectResult;
//             var userResult = res?.Value as HTTPResponseClient<UserLoginResultVM>;
//                //Tạo cookie từ server 
//             var cookieOption =  new CookieOptions(){
//                 HttpOnly = true,
//                 Secure = true,
//                 Expires = DateTime.Now.AddDays(1)
//             };
//             HttpContext.Response.Cookies.Append("accessToken",userResult.Data.AccessToken,cookieOption );
//             Console.WriteLine(@$"token :{ userResult.Data.AccessToken}");
//             return res;
//         }

//         [HttpGet("GetCookie")]
//         public async Task<ActionResult> GetCookie(){
//             string value = "";
//             bool rs = HttpContext.Request.Cookies.TryGetValue("accessToken", out value);
//             return Ok(value);
//         }


//         [Authorize]
//         [HttpGet("/user/GetProfile")]
//         public async Task<ActionResult> GetProfile([FromHeader] string authorization)
//         {
//             // string token = authorization.Replace("Bearer ", "");
//             // // string token  = HttpContext.Request.Headers["Authorization"];
//             // string account = _jwtService.DecodePayloadToken(token);
//             // var user = _context.Users.SingleOrDefault(us => us.Username == account || us.Email == account);
//             // return Ok(user);
//             return Ok("");
//         }

//         [Authorize(Roles = "Buyer,Seller")]
//         [HttpGet("/user/PostNew")]
//         public async Task<ActionResult> PostNew()
//         {

//             return Ok("");
//         }

//     }
// }



