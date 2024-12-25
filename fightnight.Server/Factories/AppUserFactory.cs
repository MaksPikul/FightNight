using fightnight.Server.Dtos.Account;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Factories
{
    public class AppUserFactory
    {
        public static AppUser Create(RegisterDto dto)
        {
            return new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = false
            };
        }
    }
}
