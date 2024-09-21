using fightnight.Server.Dtos.Member;
using fightnight.Server.Enums;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces.IRepos
{
    public interface IMemberRepo
    {
        Task<AppUserEvent> AddMemberToEvent(AppUserEvent userEvent);

        Task<AppUserEvent> RemoveMemberFromEvent(AppUserEvent aue);

        Task<List<ReturnMemberDto>> GetEventMembers(string eventId);
    }
}
