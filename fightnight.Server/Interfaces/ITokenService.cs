using fightnight.Server.Models.Tables;
using System.Security.Claims;

namespace fightnight.Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(List<Claim> claims);
        IEnumerable<Claim> DecodeToken(string token);
    }
}
