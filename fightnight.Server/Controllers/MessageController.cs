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

namespace fightnight.Server.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMemberRepo _memberRepo;
        private readonly IEventRepo _eventRepo;
        private readonly IMessageRepo _messageRepo;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;
        public MessageController(
            IEventRepo eventRepo,
            UserManager<AppUser> userManager,
            IMemberRepo memberRepo,
            IHubContext<ChatHub> hubContext,
            IMessageRepo messageRepo) 
        {
            _userManager = userManager;
            _messageRepo = messageRepo;
            _eventRepo = eventRepo;
            _memberRepo = memberRepo;
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
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            //check if user is associated with event
            var result = await _memberRepo.CheckIfMemberAsync(appUser.Id, msgBody.eventId);
            if (result)
            {
                var newMsg = new Message
                {
                    message = msgBody.msg,
                    userId = appUser.Id,
                    username = msgBody.username,
                    //userPicture = msg.userPicture,
                    eventId = msgBody.eventId
                };
                var x = await _messageRepo.CreateMessageAsync(newMsg);
                //return Ok(newMsg);
                Console.WriteLine(x);

                if (newMsg == null) return StatusCode(500, "Could not add AppUserEvent to DB");

                //add message to redis and update

                await _hubContext.Clients
                    .Group(msgBody.eventId)
                    .SendAsync("SendMsgRes", newMsg.ReturnMessageMapper());
                
                return Ok(newMsg.ReturnMessageMapper());
            }
            else
            {
                return Unauthorized("You are unauthorized to run this action, not a member of the event");
            }
        }

        [HttpGet("{eventId}")]
        [Authorize]
        public async Task<IActionResult> GetEventMessages([FromRoute] string eventId)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);
            
            var result = await _memberRepo.CheckIfMemberAsync(appUser.Id, eventId);
                
            if (result)
            {
                //check redis
                var messages =  await _messageRepo.GetMessagesAsync(eventId);
                //if (messages == null) { return BadRequest("No Events to display"); }
                return Ok(messages);
            }
            else
            {
                return Unauthorized("You are unauthorized to run this action, not a member of the event");
            }
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
            return Ok(message);
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageBody msgBody)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var ueRole = _eventRepo.GetUserEventRole(appUser.Id, msgBody.eventId);
            if (ueRole == null) return Unauthorized("You are unauthorized to run this action.");

            var message = await _messageRepo.GetMessageAsync(msgBody.msgId);
            if (message == null) return BadRequest("Message Not Found");

            await _messageRepo.DeleteMessageAsync(message);
            return Ok("Message Deleted");
        }
    }
}
