using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using CaptonseProject.Models;

namespace CaptonseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatLocalController : ControllerBase
    {
        private readonly IChatLocalServiceBE _chatLocalServiceBE;
        public ChatLocalController(IChatLocalServiceBE chatLocalServiceBE)
        {
            _chatLocalServiceBE = chatLocalServiceBE;
        }

        [Authorize(Roles = RoleConstant.Doctor + "," + RoleConstant.Receptionist)]
        [HttpGet("GetAllChatRoom")]
        public async Task<dynamic> GetAllChatRoom([FromHeader] string authorization)
        {
            var result = await _chatLocalServiceBE.GetAllChatRoom(authorization);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor + "," + RoleConstant.Receptionist)]
        [HttpPost("GetAllMessageChatFromChatRoom")]
        public async Task<dynamic> GetAllMessageChatFromChatRoom([FromBody] Guid chatID)
        {
            var result = await _chatLocalServiceBE.GetAllMessageChatFromChatRoom(chatID);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor + "," + RoleConstant.Receptionist)]
        [HttpPost("CreateChatRoom")]
        public async Task<dynamic> CreateChatRoom([FromHeader]string authorization, [FromBody] ChatBoxLocalCreateVM item)
        {
            var result = await _chatLocalServiceBE.CreateChatRoom(authorization, item);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor + "," + RoleConstant.Receptionist)]
        [HttpPost("SendMessageChatRoom")]
        public async Task<dynamic> SendMessageChatRoom([FromHeader]string authorization, [FromBody] MemberChatLocalSendVM item)
        {
            var result = await _chatLocalServiceBE.SendMessageChatRoom(authorization,item);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor + "," + RoleConstant.Receptionist)]
        [HttpDelete("DeleteChatRoom/{chatID}")]
        public async Task<dynamic> DeleteChatRoom([FromRoute] Guid chatID)
        {
            var result = await _chatLocalServiceBE.DeleteChatRoom(chatID);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstant.Doctor + "," + RoleConstant.Receptionist)]
        [HttpPost("GetAllUserForChatBoxLocal")]
        public async Task<dynamic> GetAllUserForChatBoxLocal([FromHeader]string authorization, [FromBody] PagedResponse<ConditionFilterUserSelectedForChatBoxLocal> pagedResponse)
        {
            var result = await _chatLocalServiceBE.GetAllUserForChatBoxLocal(authorization, pagedResponse);
            return Ok(result);
        }
   }
}