using fightnight.Server.Dtos.Account;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces
{
    public interface IAuthService
    {
        Task<AppUser> RegisterUserAsync(AppUser appUser, RegisterDto registerDto);
        Task<AppUser> LogUserInAsync();
    }
}
