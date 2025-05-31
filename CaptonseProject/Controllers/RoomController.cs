using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
//using CaptonseProject.Models;

namespace CaptonseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = RoleConstant.Doctor + "," + RoleConstant.Technician)]
        [HttpPost("GetAllRoomVMAsync")]
        public async Task<IActionResult> GetAllRoomVMAsync([FromBody] PagedResponse<string> pagedResponseSearchText)
        {
            var result = await _roomService.GetAllRoomVMAsync(pagedResponseSearchText);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor + "," + RoleConstant.Technician)]
        [HttpPost("GetRoomVMByIDAsync")]
        public async Task<IActionResult> GetRoomVMByIDAsync([FromBody] int roomID)
        {
            var result = await _roomService.GetRoomVMByIDAsync(roomID);
            return Ok(result);
        }
    }
}