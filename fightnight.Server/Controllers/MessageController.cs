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

namespace fightnight.Server.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMemberRepo _memberRepo;
        private readonly IMessageRepo _messageRepo;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;
        public MessageController(
            UserManager<AppUser> userManager,
            IMemberRepo memberRepo,
            IHubContext<ChatHub> hubContext,
            IMessageRepo messageRepo) 
        {
            _userManager = userManager;
            _messageRepo = messageRepo;
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
            var result = await _memberRepo.CheckIfMember(appUser.Id, msgBody.eventId);

            if (!result.Equals(null))
            {

                var newMsg = new Message
                {
                    message = msgBody.msg,
                    userId = appUser.Id,
                    username = msgBody.username,
                    //userPicture = msg.userPicture,
                    eventId = msgBody.eventId
                };
                await _messageRepo.CreateMessage(newMsg);

                //add message to redis and update

                await _hubContext.Clients.Group(msgBody.eventId).SendAsync("SendMsgReq", newMsg);

                return Ok(newMsg);

            }
            else
            {
                return BadRequest("User Not Associated with Event");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEventMessages([FromQuery] string eventId)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var isMember = await _memberRepo.CheckIfMember(appUser.Id, eventId);
            if (!isMember.Equals(null))
            {
                //check redis


                var messages = _messageRepo.GetMessages(eventId);
                if (messages == null)
                {
                    return NoContent();
                }
                return Ok(messages);
            }
            else
            {
                return BadRequest("");
            }
            
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> EditMessage()
        {
            return BadRequest("");
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteMessage()
        {
            return BadRequest("not yet");
        }
    }
}
