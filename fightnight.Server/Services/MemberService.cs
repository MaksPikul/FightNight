using fightnight.Server.Dtos.Member;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Models.Tables;
using fightnight.Server.Repos;

namespace fightnight.Server.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepo _memberRepo;
        public MemberService(
            IMemberRepo memberRepo
        ) {
            _memberRepo = memberRepo;
        }
        public async Task<MemberResDto> GetEventMemberProfileByEmailAsync(string userEmail, string eventId)
        {
            MemberResDto member = await _memberRepo.GetEventMemberProfileAsync(userEmail, eventId);
            return member;
        }

        public void AddUserToEvent(Invitation invite, AppUser invitedUser)
        {
            var appUserEvent = new AppUserEvent
            {
                EventId = invite.eventId,
                AppUserId = invitedUser.Id,
                Role = invite.proposedRole,
            };

            _memberRepo.AddMemberToEventAsync(appUserEvent);

        }

        public void RemoveMemberFromEvent(AppUserEvent member)
        {
            _memberRepo.RemoveMemberFromEventAsync(member);
        }
    }
}
