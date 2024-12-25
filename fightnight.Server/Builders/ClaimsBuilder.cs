using System.Security.Claims;

namespace fightnight.Server.Builders
{
    public class ClaimsBuilder
    {
        private readonly List<Claim> _claimsList = new();

        public ClaimsBuilder AddClaim(string type, string value)
        {
            _claimsList.Add(new Claim(type, value));
            return this; 
        }

        public List<Claim> Build()
        {
            return _claimsList; 
        }
    }
}
