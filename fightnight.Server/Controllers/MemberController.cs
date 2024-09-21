using fightnight.Server.Data;
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
            var ueRole = _eventRepo.GetUserEventRoleAsync(appUser.Id, newMemberDto.eventId);

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

            var result = await _memberRepo.AddMemberToEvent(AppUserEvent);
            if (AppUserEvent == null) return StatusCode(500, "Could not add AppUserEvent to DB");

            return Ok("");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveMemberFromEvent()
        {
            return BadRequest("Not implemented");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEventMembers()
        {
            return BadRequest("Not implemented");
        }

    }
}
