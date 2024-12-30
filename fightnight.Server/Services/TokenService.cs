using fightnight.Server.Builders;
using fightnight.Server.Interfaces;
using fightnight.Server.Models.Tables;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace fightnight.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public TokenService(IConfiguration config) { 
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string CreateToken(List<Claim> claims) 
        {

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                //Expires = DateTime.Now.AddDays(7),      // Keep, or set manually with claims builder?
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);

            return _tokenHandler.WriteToken(token);
        }

        public IEnumerable<Claim> DecodeToken(string token)
        {
            var jwt = _tokenHandler.ReadJwtToken(token);

            var claims = jwt.Claims;

            return claims;
        }


        public bool ValidateToken(string token)
        {
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],
                ValidateLifetime = true
            };
            try
            {
                SecurityToken validatedToken;
                _tokenHandler.ValidateToken(token, validationParams, out validatedToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
       }
    }
}
