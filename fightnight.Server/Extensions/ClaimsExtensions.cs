using System.Security.Claims;

namespace fightnight.Server.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            var emailClaim = user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email);
            return emailClaim?.Value;
        }
    }
}
