using fightnight.Server.Dtos.Member;
using fightnight.Server.Enums;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces.IRepos
{
    public interface IMemberRepo
    {
        Task<AppUserEvent> AddMemberToEventAsync(AppUserEvent userEvent);

        Task<AppUserEvent> RemoveMemberFromEventAsync(AppUserEvent aue);

        Task<List<ReturnMemberDto>> GetEventMembersAsync(string eventId);
        Task<AppUserEvent> GetMemberAsync(string userId, string eventId);

        Task<Boolean> CheckIfMemberAsync(string userId, string eventId);
    }
}
