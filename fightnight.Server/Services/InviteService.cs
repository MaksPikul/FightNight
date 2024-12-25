using Azure;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Models.Tables;
using fightnight.Server.Repos;

namespace fightnight.Server.Services
{
    public class InviteService : IInviteService
    {
        private readonly IInviteRepo _inviteRepo;
        private readonly IMemberRepo _memberRepo;
        public InviteService(
            IInviteRepo inviteRepo,
            IMemberRepo memberRepo
        ) {
            _inviteRepo = inviteRepo;
            _memberRepo = memberRepo;
        }


        //
        public async void HandleInvitation(AppUser appUser, string inviteId, HttpResponse response)
        {
            Invitation invite = await _inviteRepo.GetInvitationAsync(inviteId);

            if (invite == null)
            {
                return;
            }
            appUser.EmailConfirmed = true;
            response.Redirect("https://localhost:5173/" + invite.eventId + "/team");

            var AppUserEvent = new AppUserEvent
            {
                EventId = invite.eventId,
                AppUserId = appUser.Id,
                Role = EventRole.Moderator,
            };

            await _memberRepo.AddMemberToEventAsync(AppUserEvent);

            return;
        }








    }
}
