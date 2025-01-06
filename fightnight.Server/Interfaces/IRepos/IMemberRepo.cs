using fightnight.Server.Dtos.Member;
using fightnight.Server.Enums;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces.IRepos
{
    public interface IMemberRepo
    {
        Task<AppUserEvent> AddMemberToEventAsync(AppUserEvent userEvent);

        Task<AppUserEvent> RemoveMemberFromEventAsync(AppUserEvent aue);

        Task<MemberResDto> GetEventMemberProfileAsync(string email, string eventId);

    }
}
