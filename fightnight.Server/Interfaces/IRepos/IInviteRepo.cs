﻿using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces.IRepos
{
    public interface IInviteRepo
    {
        Task<Invitation> AddInviteAsync(Invitation invitation); 
        Task<Invitation> UpdateInviteAsync(Invitation invitation);
        Task<Invitation> DeleteInviteAsync(Invitation invitation);
        Task<Invitation> GetInvitationByIdAsync(string Id);
        Task<Invitation> GetInvitationByEmailAsync(string Email);
    }
}
