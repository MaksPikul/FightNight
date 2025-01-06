using fightnight.Server.Dtos.Member;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces
{

    public interface IMemberService
    {
        Task<MemberResDto> GetEventMemberProfileByEmailAsync(string userId, string eventId);
        Task<AppUserEvent> GetAppUserEvent(string userId, string eventId);
        void AddUserToEvent(Invitation invite, AppUser invitedUser);
        void RemoveMemberFromEvent(AppUserEvent member);
    }
}
