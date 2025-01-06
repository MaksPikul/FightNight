using fightnight.Server.Dtos.Message;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using fightnight.Server.Interfaces.IRepos;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using fightnight.Server.Hubs;
using fightnight.Server.Extensions;
using Microsoft.AspNetCore.Identity;
using fightnight.Server.Mappers;
using fightnight.Server.Dtos.User;
using fightnight.Server.repo;
using fightnight.Server.Interfaces;
using fightnight.Server.Services;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Factories;

namespace fightnight.Server.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IEventService _eventService;
        private readonly IMessageRepo _messageRepo;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;
        public MessageController(
            IEventService eventService,
            UserManager<AppUser> userManager,
            IMemberService memberService,
            IHubContext<ChatHub> hubContext,
            IMessageRepo messageRepo) 
        {
            _userManager = userManager;
            _messageRepo = messageRepo;
            _eventService = eventService;
            _memberService = memberService;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddMessage([FromBody] AddMessageBody msgBody)
        {
            if (msgBody.msg.Trim() == "")
            {
                return BadRequest("Message is empty");
            }

            var appUser = await _userManager.FindByEmailAsync( User.GetEmail() );

   
            MemberResDto existingMember = await _memberService.GetEventMemberProfileByEmailAsync(appUser.Email, msgBody.eventId);
            if (existingMember == null)
            {
                return Unauthorized("You are unauthorized to run this action, not a member of the event");
            }

            Message newMsg = MessageFactory.CreateMessage(msgBody, appUser.Id);

            var x = await _messageRepo.CreateMessageAsync(newMsg);

            if (newMsg == null) return StatusCode(500, "Could not add AppUserEvent to DB");

            await _hubContext.Clients
                .Group(msgBody.eventId)
                .SendAsync("SendMsgRes", newMsg.ReturnMessageMapper());
                
            return Ok(newMsg.ReturnMessageMapper());
            
        }



        [HttpGet("{eventId}/{offset}/{limit}")]
        [Authorize]
        public async Task<IActionResult> GetEventMessages([FromRoute] string eventId, int offset=0, int limit=50)
        {
            var appUser = await _userManager.FindByEmailAsync( User.GetEmail() );

            MemberResDto existingMember = await _memberService.GetEventMemberProfileByEmailAsync(appUser.Email, eventId);
            if (existingMember == null)
            {
                return Unauthorized("You are unauthorized to run this action, not a member of the event");
            }
            

            var messages =  await _messageRepo.GetMessagesAsync(eventId, offset, limit);
            return Ok(messages);
            
            
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> EditMessage([FromBody] EditMessageBody msgBody)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var message = await _messageRepo.GetMessageAsync(msgBody.msgId);
            if (message == null) return BadRequest("Message Not Found");

            if (message.userId != appUser.Id) return Unauthorized("You are unauthorized to run this action, you are not message owner");

            // message exists, user requested change
            message.message = msgBody.newMsg;
            message.IsEdited = true;
            await _messageRepo.UpdateMessageAsync(message);
            //if error, return it

            await _hubContext.Clients
                    .Group(msgBody.eventId)
                    .SendAsync("EditMsgRes", message);


            return Ok("Message Edited");
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageBody msgBody)
        {
            AppUser appUser = await _userManager.FindByEmailAsync( User.GetEmail() );

            bool ueRole = _eventService.IsEventRoleValid(Enums.EventRole.Admin, appUser.Id, msgBody.eventId);

            if (ueRole)
            {
                return Unauthorized("You are unauthorized to run this action.");
            }

            Message message = await _messageRepo.GetMessageAsync(msgBody.msgId);
            if (message == null)
            {
                return BadRequest("Message Not Found");
            }

            await _hubContext.Clients
                    .Group(msgBody.eventId)
                    .SendAsync("DeleteMsgRes", msgBody.msgId);

            await _messageRepo.DeleteMessageAsync(message);
            return Ok("Message Deleted");
        }
    }
}
