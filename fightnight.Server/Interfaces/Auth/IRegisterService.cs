using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Identity;

namespace fightnight.Server.Interfaces.Auth
{
    public interface IRegisterService
    {
        Task<AppUser> AddUnconfirmedUserAsync(AppUser appUser, string password = null);
        Task<AppUser> GetUnconfirmedUserAsync(string email);
        Task<IdentityResult> ConfirmUserAsync(AppUser appUser);
    }
}
