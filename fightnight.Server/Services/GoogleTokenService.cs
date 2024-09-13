using System.Diagnostics;
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using fightnight.Server.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Any;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace fightnight.Server.Services
{
    public class GoogleTokenService : IGoogleTokenService
    {
        
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        static readonly HttpClient client = new HttpClient();
        public GoogleTokenService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        class OAuthReq
        { 
        public string code { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string redirect_uri { get; set; }
        public string grant_type { get; set; }
        };

        public class GoogleTokenResult
        {
        public string access_token { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        }

        public class GoogleUser
        {
            //public string Id { get; set; }
            public string name { get; set; }
            //public string GivenName { get; set; }
            //public string FamilyName { get; set; }
            //public string Profile { get; set; }
            public string picture { get; set; }
            public string email { get; set; }
            public bool email_verified { get; set; }
            //public string Gender { get; set; }
            //public string Locale { get; set; }
        }


        //Need this
        public async Task<GoogleTokenResult> getGoogleOAuthTokens(string Code)
        {
            string url = "https://oauth2.googleapis.com/token";


            using HttpResponseMessage response = await client
                .PostAsJsonAsync(
                url,
                new OAuthReq {
                code = Code,
                client_id = _configuration["Auth:Google:ClientId"],               //process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID,
                client_secret = _configuration["Auth:Google:ClientSecret"],                                                 //process.env.NEXT_PUBLIC_GOOGLE_CLIENT_SECRET,
                redirect_uri = "https://localhost:7161/api/account/oauth/google",                                      //process.env.NEXT_PUBLIC_GOOGLE_OAUTH_OAUTH_REDIRECT_URL,
                grant_type = "authorization_code"
                });

            response.EnsureSuccessStatusCode();

            var tokens = await response.Content.ReadFromJsonAsync<GoogleTokenResult>();

            return tokens;
        }

        //Might not need this
        public async Task<HttpResponseMessage> getGoogleUser(string id_token, string access_token)
        {
            
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , id_token);

            using HttpResponseMessage response = await client
                .GetAsync("https://www.googleapis.com/oauthv2/v1/userinfo?=json&access_token=$"+access_token);

            response.EnsureSuccessStatusCode();

            //var user = await response.Content.ReadFromJsonAsync<GoogleUser>();

            return response;
        }
    }
}