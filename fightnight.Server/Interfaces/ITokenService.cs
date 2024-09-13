using fightnight.Server.Models;
using System.Security.Claims;

namespace fightnight.Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        IEnumerable<Claim> DecodeToken(string token);
    }
}
