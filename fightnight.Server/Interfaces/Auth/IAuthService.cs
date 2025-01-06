using fightnight.Server.Dtos.Account;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace fightnight.Server.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AppUser> AddUnconfirmedUserAsync(AppUser appUser, string password = null);
        Task<AppUser> GetUnconfirmedUserAsync(string email);
        Task<IdentityResult> ConfirmUserAsync(AppUser appUser);
        Task<AppUser> LogUserInAsync(AppUser appUser, ClaimsPrincipal user, string password = null, bool rememberMe = false);
        void LogUserOutAsync();
    }
}
