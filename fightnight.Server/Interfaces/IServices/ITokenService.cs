using fightnight.Server.Models.Tables;
using System.Security.Claims;

namespace fightnight.Server.Interfaces.IServices
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        IEnumerable<Claim> DecodeToken(string token);
    }
}
