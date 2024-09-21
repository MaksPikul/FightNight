using Microsoft.AspNetCore.Mvc;
using static fightnight.Server.Services.GoogleTokenService;

namespace fightnight.Server.Interfaces.IServices
{
    public interface IGoogleTokenService
    {
        Task<GoogleTokenResult> getGoogleOAuthTokens(string code);
        //Task<GoogleUser> 

        Task<HttpResponseMessage> getGoogleUser(string id_token, string access_token);
    }
}
