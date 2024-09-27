using Amazon.S3.Model;
using fightnight.Server.Data;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.NewFolder;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Extensions;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models;
using fightnight.Server.Models.Tables;
using fightnight.Server.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


        // Create Invite Link, those who click link will join, "What if no account?"
        // Have invite link in url, so after login, they can join?

        // Invite Code, if you have code, go to "Enter event as Mod / fighter"

        // Send invitiation by searching up email,
        // if no account, send email to notify theyve been invited, and for them to make an account, invite will be in the 
        // admin or those who send invites needs to be able to see who hes invited, invites after a certain time, need to be deleted, max 30 days.

        // If account already exists, theyll see invite in their notifications


        // join by entering code, User clicks on button in home, join event with code, user enters code and is redirected

        // join by clicking link, 
        // join by accepting invite, chekc if user in invites, 


        //invite sent by email, shows up in user notifications once they create account, user can click the link sent in email or go to their notifications and click to accept
        // if email doesnt own account, redirect to register, once they register, their invite will be in their notifications


        //Create invite which is sent out to users,
        [HttpPost("invite")]
        [Authorize]
        public async Task<IActionResult> SendInvite([FromBody] string userId, string eventId, EventRole role, int Expiration)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);
            var ueRole = _eventRepo.GetUserEventRole(appUser.Id, eventId);

            if (!ueRole.Equals(EventRole.Admin))
            {
                return Unauthorized("You are unauthorized to complete this action");
            }
            // requesting user authorised and in event

            var invite = new Invitation
            {
                userId = userId,
                eventId = eventId,
                expiration = DateTime.UtcNow.AddDays(Expiration),
                proposedRole = role,
            };

            await _inviteRepo.AddInviteAsync(invite);

            // if invite sent successfully,

            //Create Email
            await _emailService.Send("email");

            //if user doesnt accept in 30 days
            return Ok("An invitation has been sent to this email.");
        }


        // user clicks link, redirect to one of the pages
        [HttpPost("join-w-link")]
        public async Task<IActionResult> JoinWithLink([FromBody] string codeInLink)
        {

        }




        // user enters code in home page
        [HttpPost("join-w-code")]
        [Authorize]
        public async Task<IActionResult> JoinWithCode([FromBody] string code, EventRole propRole)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);


            // get event with code
            var eventVar = _eventRepo.GetEventWithCode(code);

            if (eventVar == null) return NotFound("Event with this join code doesn't exist");

            var AppUserEvent = new AppUserEvent
            {
                EventId = appUser.Id,
                AppUserId = eventVar.eventId,
                Role = propRole,
            };

            await _memberRepo.AddMemberToEventAsync(AppUserEvent);
            //Check if failed, 
            //Check if already a member

            return Ok(
                message:"You have joined the event as " + propRole,
                eventId: eventVar.id // for redirect
            );


        }


        //admin asks to generate new code, CODE FIRST GENERATED ON EVENT CREATION
        



        [HttpPost("join")]
        [Authorize]
        public async Task<IActionResult> AddMemberToEvent([FromBody] NewMemberDto newMemberDto)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);
            var ueRole = _eventRepo.GetUserEventRole(appUser.Id, newMemberDto.eventId);

            if (!ueRole.Equals(EventRole.Admin))
            {
                return Unauthorized("You are unauthorized to complete this action");
            }

            // may not need this ---
            var existingUser = _userManager.FindByIdAsync(newMemberDto.newMemberId);
            if (existingUser == null)
            {
                return BadRequest("User Does not exist");
            }
            // ---------------------

            var exisitngUserInTeam = _context.AppUserEvent.FirstOrDefault(ue => 
                ue.AppUserId == newMemberDto.newMemberId 
                && 
                ue.EventId == newMemberDto.eventId
            );
            if (exisitngUserInTeam == null)
            {
                return BadRequest("User already added to event");
            }
            var AppUserEvent = new AppUserEvent
            {
                EventId = newMemberDto.newMemberId,
                AppUserId = newMemberDto.eventId,
                Role = newMemberDto.newRole,
            };

            var result = await _memberRepo.AddMemberToEventAsync(AppUserEvent);
            if (AppUserEvent == null) return StatusCode(500, "Could not add AppUserEvent to DB");

            return Ok("");
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
            return Ok(member.AppUser.UserName + " Has been removed from event");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEventMembers([FromBody] string eventId)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var result = await _memberRepo.CheckIfMemberAsync(appUser.Id, eventId);
            if (result) return Unauthorized("Unauthorized, not a member of event.");

            var members = await _memberRepo.GetEventMembersAsync(eventId);
            return Ok(members);
        }

    }
}
