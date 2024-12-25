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
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var emailClaim = user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name);
            return emailClaim?.Value;
        }

        public static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
        {
            var claim = claims.FirstOrDefault(c => c.Type == claimType).Value;

            return claim;
        }
    }
}
