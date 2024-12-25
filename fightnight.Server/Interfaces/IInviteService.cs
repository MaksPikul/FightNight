using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces
{
    public interface IInviteService
    {
        void HandleInvitation(AppUser appUser, string inviteId, HttpResponse response);
    }
}
