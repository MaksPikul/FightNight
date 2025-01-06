using MetInProximityBack.Types;

namespace fightnight.Server.Interfaces.Auth
{
    public interface IOAuthService
    {
        Task<OAuthTokenResponse> GetOAuthTokens(string url, Dictionary<string, string> req);
        //Task<HttpResponseMessage> GetUserAsResponse(string url, string accessToken);
    }
}
