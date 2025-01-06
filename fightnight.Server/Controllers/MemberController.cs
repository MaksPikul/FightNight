using Amazon.S3.Model;
using fightnight.Server.Abstracts;
using fightnight.Server.Data;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Extensions;
using fightnight.Server.Factories;
using fightnight.Server.Hubs;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models;
using fightnight.Server.Models.Tables;
using fightnight.Server.Providers.EmailProviders;
using fightnight.Server.Repos;
using fightnight.Server.Services;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks.Dataflow;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace fightnight.Server.Controllers
{
    [Route("/api/member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ICacheService _cacheService;
        private readonly IMemberService _memberService;
        private readonly IInviteService _inviteService;
        private readonly IEmailService _emailService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;
        public MemberController(
        IEventService eventService,
        ICacheService cacheService,
        IMemberService memberService,
        IInviteService inviteService,
        IEmailService emailService,
        IHubContext<ChatHub> hubContext,
        UserManager<AppUser> userManager
        ) { 
            _eventService = eventService;
            _cacheService = cacheService;
            _memberService = memberService;
            _userManager = userManager;
            _emailService = emailService;
            _hubContext = hubContext;
            _inviteService = inviteService; 
    }

        [HttpPost("invite")]
        [Authorize]
        public async Task<IActionResult> SendInvite([FromBody] InviteMemberReqDto sendInvBody)
        {
            var appUser = await _userManager.FindByEmailAsync( User.GetEmail() );

            // get event i think
            
            bool validRole = _eventService.IsEventRoleValid(EventRole.Admin, appUser.Id, sendInvBody.eventId);

            if (validRole)
            {
                return Unauthorized("You are unauthorized to complete this action");
            }

            Invitation invite = await _inviteService.GetInviteByEmailAsync(sendInvBody.newMemberEmail);
            if (invite != null)
            {
                return BadRequest("Invite Already Sent");
            }

            // No need to check if user exists, if it doesnt, then it wont show up here, becasue a userId and EventId must be valid
            MemberResDto existingMember = await _memberService.GetEventMemberProfileByEmailAsync(sendInvBody.newMemberEmail, sendInvBody.eventId);
            if (existingMember != null)
            {
                return BadRequest("User already a member of event");
            }

            invite = InvitationFactory.CreateInvite(sendInvBody);
            _inviteService.AddInviteToDb(invite);

            // if error, return error
            // if invite sent successfully,

            Abstracts.Email email = new EventInviteEmail(existingMember.Email, invite.Id);
            await _emailService.SendEmail(email);

            // Currently for debugging and testing, 
            string link = "https://localhost:5173/eventInvite?token=" + invite.Id + "&email=" + email;
            return Ok(link);
        }



        // Make one controller which handles invites in general? Using Chain Design Pattern, GOOD IDEA?


        // user clicks link in email or clicks Accept in home page, redirect to invite page, user clicks JOIN 
        [HttpPost("join-w-invite")]
        public async Task<IActionResult> JoinWithInvite([FromBody] string inviteId)
        {
            Invitation invite = await _inviteService.GetInviteByIdAsync(inviteId);

            if (invite == null)
            {
                return NotFound("Invitation not found");
            }
            else if (invite.expiration > DateTime.Now)
            {
                _inviteService.DeleteInviteAsync(invite);
                return BadRequest("Invitation has expired, Ask for another.");
            }

            //checks if user exsists
            AppUser invitedUser = await _userManager.FindByEmailAsync(invite.userEmail);
            if (invitedUser == null) return Redirect("https://localhost:5173/register?inviteId=" + invite.Id);
            
            //checks if user logged in
            var loggedInUserEmail = User.GetEmail();
            if (loggedInUserEmail == null) return Redirect("https://localhost:5173/login?inviteId=" + invite.Id);

            //User logged in, so hes redirected to event team page
            _memberService.AddUserToEvent(invite, invitedUser);

            _inviteService.DeleteInviteAsync(invite);

            return Redirect("https://localhost:5173/event/" + invite.eventId + "/team");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveMemberFromEvent([FromBody] RemoveMemberReqDto mbrBody)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            AppUserEvent member = await _memberService.GetAppUserEvent(mbrBody.userId, mbrBody.eventId);
            if (member == null) return BadRequest("Member or Event does not exist");

            if (member.AppUserId == appUser.Id)
            {
                if (member.Role.Equals(EventRole.Admin))
                {
                    return BadRequest("Unable to remove the admin from event");
                }

                _memberService.RemoveMemberFromEvent(member);
                return Redirect("https://localhost:5173/home");
            }
            _memberService.RemoveMemberFromEvent(member);

            // * notification for them to close event, becasue removed
            string connectionKey = $"chat/event:{member.EventId}/user:{member.AppUserId}";
            string connectionId = await _cacheService.GetFromCacheAsync(connectionKey);
            _cacheService.RemoveFromCacheAsync(connectionKey);

            await _hubContext.Clients.Client(connectionId).SendAsync("CloseConnection", member.EventId);

            return Ok(member.AppUser.UserName + " Has been removed from event");
        }





        // user clicks shared link ,  Will do later, Focus on Invite Join

        /*
         * 
        
        No Need for this, will keep, 
        Not necessary cause users are fetched with event,
        Might be necessary when refetching and having cached data? fresh event data, stale members, fetch only memebrs?

         
        [HttpGet("{eventId}")]
        [Authorize]
        public async Task<IActionResult> GetEventMembers([FromRoute] string eventId)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var boolResult = await _memberRepo.CheckIfMemberAsync(appUser.Id, eventId);
            if (!boolResult) return Unauthorized("Unauthorized, not a member of event.");

            var members = await _memberRepo.GetEventMembersAsync(eventId);
            return Ok(members);
        }


        [HttpPost("join-w-share")]
        public async Task<IActionResult> JoinWithSharedLink([FromBody] string eventJoinCode)
        {
            var eventVar = await _eventRepo.GetEventWithJoinCodeAsync(eventJoinCode);
            if (eventVar == null) return NotFound("Event with this join code doesn't exist");

            var loggedInUserEmail = User.GetEmail();
            if (loggedInUserEmail == null) return Redirect("https://localhost:5173/login?inviteId=" + eventJoinCode);

            var appUser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            var AppUserEvent = new AppUserEvent
            {
                EventId = eventVar.Id,
                AppUserId = appUser.Id,
                Role = EventRole.Moderator,
            };

            var result = await _memberRepo.CheckIfMemberAsync(appUser.Id, eventVar.Id);
            if (result == true) return BadRequest("Already a member");

            await _memberRepo.AddMemberToEventAsync(AppUserEvent);
            //Check if failed

            return Redirect("https://localhost:5173/event/" + eventVar.Id + "/team");
        }
        */

    }
}
