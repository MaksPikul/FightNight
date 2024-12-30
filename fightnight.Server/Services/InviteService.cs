using Azure;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Models.Tables;
using fightnight.Server.Repos;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Reflection.Metadata;

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

        public async Task<Invitation> UpdateUserAsync(AppUser appUser, string inviteId, HttpResponse response)
        {
            Invitation invite = await _inviteRepo.GetInvitationAsync(inviteId);

            if (invite == null)
            {
                return null;
            }
            appUser.EmailConfirmed = true;
            response.Redirect("https://localhost:5173/" + invite.eventId + "/team");

            return invite;
        }

        public async Task<Invitation> AddUserToEventAsync(Invitation invite)
        {
            if (invite == null)
            {
                return null;
            }

            var AppUserEvent = new AppUserEvent
            {
                EventId = invite.eventId,
                AppUserId = invite.AppUser.Id,
                Role = EventRole.Moderator,
            };

            var res = await _memberRepo.AddMemberToEventAsync(AppUserEvent);
            if (res == null)
            {
                throw new Exception("Failed to add user to event");
            }

            return invite;
        }
    }
}
