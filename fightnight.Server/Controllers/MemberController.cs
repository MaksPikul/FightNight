using fightnight.Server.Data;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.NewFolder;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Extensions;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Models;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fightnight.Server.Controllers
{
    [Route("/api/member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IEventRepo _eventRepo;
        private readonly IMemberRepo _memberRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDBContext _context;
        public MemberController(
            IEventRepo eventRepo,
            IMemberRepo memberRepo,
            UserManager<AppUser> userManager,
            AppDBContext context
        ) { 
            _eventRepo = eventRepo;
            _memberRepo = memberRepo;
            _userManager = userManager;
            _context = context;
        }

        

        [HttpPost]
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

            var role = _eventRepo.GetUserEventRole(appUser.Id, mbrBody.eventId);
            if (!role.Equals(EventRole.Admin)) return Unauthorized("Unauthorized, you do not have permissions to run action");

            var member = await _memberRepo.GetMemberAsync(mbrBody.userId, mbrBody.eventId);
            if (member == null) return BadRequest("Member or Event does not exist");

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
