using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces
{
    public interface IInviteService
    {
        Task<Invitation> UpdateUserAsync(AppUser appUser, string inviteId, HttpResponse response);
        Task<Invitation> AddUserToEventAsync(Invitation invite);
    }
}
