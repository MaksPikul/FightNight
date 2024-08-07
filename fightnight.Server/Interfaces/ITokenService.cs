using fightnight.Server.Models;

namespace fightnight.Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
