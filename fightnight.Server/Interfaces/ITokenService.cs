using fightnight.Server.Models.Tables;
using System.Security.Claims;

namespace MetInProximityBack.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        IEnumerable<Claim> DecodeToken(string token);
    }
}
