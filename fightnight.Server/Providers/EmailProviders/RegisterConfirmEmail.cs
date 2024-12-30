using fightnight.Server.Abstracts;
using fightnight.Server.Builders;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Interfaces;
using System.Security.Claims;

namespace fightnight.Server.Providers.EmailProviders
{
    public class RegisterConfirmEmail : Email
    {
        public RegisterConfirmEmail(string email, ITokenService tokenService)
        {
            // Create Claims
            var exp = DateTime.Now.AddMinutes(5).ToString();

            List<Claim> claims = new ClaimsBuilder()
                .AddClaim("expiration", exp)
                .AddClaim("email", email)
                .Build();

            // Create JWT Token
            string token = tokenService.CreateToken(claims);

            Subject = "Confirm your Account Email";

            string emailVerifyLink = "https://localhost:5173/verify-email?token=" + token;

            Body = "<p>Click <a href=" + emailVerifyLink + ">Here</a> to verify your email.</p>";
        }
    }
}
