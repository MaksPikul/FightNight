using fightnight.Server.Dtos.Account;
using fightnight.Server.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace fightnight.Server.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public NewUserDto getSessionData(string key)
        {
            return JsonSerializer.Deserialize<NewUserDto>(_httpContextAccessor.HttpContext.Session.GetString(key));
        }
        public void setSessionData(string key, NewUserDto userDto) 
        {
            string jsonString = JsonSerializer.Serialize(userDto);
            _httpContextAccessor.HttpContext.Session.SetString(key, jsonString);
            
        }


    }
}
