using fightnight.Server.Dtos.Account;
using fightnight.Server.Models.Tables;
using System.Security.Claims;

namespace fightnight.Server.Interfaces
{
    public interface IAuthService
    {
        Task<AppUser> RegisterUserAsync(AppUser appUser, string password = null);
        Task<AppUser> LogUserInAsync(AppUser appUser, ClaimsPrincipal user, string password = null, bool rememberMe = false);
        void LogUserOutAsync();
    }
}
