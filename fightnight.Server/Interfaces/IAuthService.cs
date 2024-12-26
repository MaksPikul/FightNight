using fightnight.Server.Dtos.Account;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces
{
    public interface IAuthService
    {
        Task<AppUser> RegisterUserAsync(AppUser appUser, string password = null);
        Task<AppUser> LogUserInAsync(AppUser appUser, string password = null, bool rememberMe = false);
    }
}
