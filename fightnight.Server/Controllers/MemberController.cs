using Amazon.S3.Model;
using fightnight.Server.Data;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.NewFolder;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Extensions;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models;
using fightnight.Server.Models.Tables;
using fightnight.Server.Models.Types;
using fightnight.Server.Repos;
using fightnight.Server.Services;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks.Dataflow;

namespace fightnight.Server.Controllers
{
    [Route("/api/member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IEventRepo _eventRepo;
        private readonly IMemberRepo _memberRepo;
        private readonly IInviteRepo _inviteRepo;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDBContext _context;
        public MemberController(
        IEventRepo eventRepo,
        IMemberRepo memberRepo,
        IInviteRepo inviteRepo,
        IEmailService emailService,
        UserManager<AppUser> userManager,
        AppDBContext context
        ) { 
            _eventRepo = eventRepo;
            _memberRepo = memberRepo;
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
            _inviteRepo = inviteRepo;
        }

        /*
        Share Link - uses event joinCode
        Invite Sent by email - uses inviteId

        Fighters pasting code into search to register interest, if over fighter limit, add to waitlist

        - Logged in at home, sees notification to join, clicks accept invite
        - Click link and Logged in and it takes you to invite page, once accepted, redirects to event page
        - Logged out, takes you to register, once registered, redirects to invite page, once accepted, recirects to event page
         */

        //upon clicking invite links, redirected to a page, this page then redirects them to valid pages based on status
       
        [HttpPost("invite")]
        [Authorize]
        public async Task<IActionResult> SendInvite([FromBody] SendInviteBody sendInvBody)
        {
            var sendingUserEmail = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(sendingUserEmail);

            // get event i think
            var ueRole = _eventRepo.GetUserEventRole(appUser.Id, sendInvBody.eventId);

            if (!ueRole.Equals(EventRole.Admin))
            {
                return Unauthorized("You are unauthorized to complete this action");
            }

            var existingInvite = await _inviteRepo.InviteExistsAsync(sendInvBody.newMemberEmail);
            if (existingInvite) return BadRequest("Invite Already Sent");

            var user = await _userManager.FindByEmailAsync(sendInvBody.newMemberEmail);
            if (user != null)
            {
                var existingMember = await _memberRepo.CheckIfMemberAsync(user.Id, sendInvBody.eventId);
                if (existingMember) return BadRequest("User already a member of event");
            }

            var invite = new Invitation
            {
                userEmail = sendInvBody.newMemberEmail,
                eventId = sendInvBody.eventId,
                //expiration = DateTime.UtcNow.AddDays(Expiration),
                proposedRole = sendInvBody.role,
            };

            await _inviteRepo.AddInviteAsync(invite);
            // if error, return error
            // if invite sent successfully,
            
            string emailVerifyLink = "https://localhost:5173/eventInvite?token=" + invite.Id + "&email=" + sendInvBody.newMemberEmail;

            /*
            var email = new ConfirmEmailTemplate
            {
                SendingTo = sendInvBody.newMemberEmail,
                EmailBody = "<p>Click <a href=" + emailVerifyLink
                + ">Here</a> to join event.</p>"
            };
            await _emailService.Send(email);
            */

            return Ok(emailVerifyLink);//"If email exists, It will receive this Invite.");
        }


        // user clicks link in email or clicks Accept in home page, redirect to invite page, user clicks JOIN 
        [HttpPost("join-w-invite")]
        public async Task<IActionResult> JoinWithInvite([FromBody] string inviteId)
        {
            var invite = await _inviteRepo.GetInvitationAsync(inviteId);
            if (invite == null) return NotFound("Invitation not found");
            else if (invite.expiration > DateTime.Now) {
                await _inviteRepo.DeleteInviteAsync(invite);
                return BadRequest("Invitation has expired, Ask for another.");
             }

            //checks if user exsists
            var invitedUser = await _userManager.FindByEmailAsync(invite.userEmail);
            if (invitedUser == null) return Redirect("https://localhost:5173/register?inviteId=" + invite.Id);
            
            //checks if user logged in
            var loggedInUserEmail = User.GetEmail();
            if (loggedInUserEmail == null) return Redirect("https://localhost:5173/login?inviteId=" + invite.Id);

            //User logged in, so hes redirected to event team page
            var appUser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            var AppUserEvent = new AppUserEvent
            {
                EventId = invite.eventId,
                AppUserId = appUser.Id,
                Role = invite.proposedRole,
            };

            await _inviteRepo.DeleteInviteAsync(invite);

            return Redirect("https://localhost:5173/event/" + invite.eventId + "/team");
        }

        // user clicks shared link
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
                EventId = eventVar.id,
                AppUserId = appUser.Id,
                Role = EventRole.Moderator,
            };

            var result = await _memberRepo.CheckIfMemberAsync(appUser.Id, eventVar.id);
            if (result == true) return BadRequest("Already a member");

            await _memberRepo.AddMemberToEventAsync(AppUserEvent);
            //Check if failed

            return Redirect("https://localhost:5173/event/" + eventVar.id + "/team");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveMemberFromEvent([FromBody] RemoveMemberBody mbrBody)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var member = await _memberRepo.GetMemberAsync(mbrBody.userId, mbrBody.eventId);
            if (member == null) return BadRequest("Member or Event does not exist");

            if (member.AppUserId == appUser.Id)
            {
                if (member.Role.Equals(EventRole.Admin))
                {
                    return BadRequest("Unable to remove the admin from event");
                }

                await _memberRepo.RemoveMemberFromEventAsync(member);
                return Redirect("https://localhost:5173/home");
            }

            await _memberRepo.RemoveMemberFromEventAsync(member);

            /*
            await _hubContext.Clients.Client(connectionId).SendAsync("ForceDisconnectReq");
            */

            return Ok(member.AppUser.UserName + " Has been removed from event");
        }

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

    }
}
