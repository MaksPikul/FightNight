using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces
{
    public interface IInviteService
    {
        Task<Invitation> UpdateUserAsync(AppUser appUser, string inviteId, HttpResponse response);
        Task<Invitation> GetInviteByEmailAsync(string email);
        Task<Invitation> GetInviteByIdAsync(string inviteId);
        void AddInviteToDb(Invitation invite);
        Task<Invitation> AddUserToEventAsync(Invitation invite);
        void DeleteInviteAsync(Invitation invite);
    }
}
