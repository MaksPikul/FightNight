using fightnight.Server.Dtos.Account;

namespace fightnight.Server.Interfaces
{
    public interface ISessionService
    {
        NewUserDto getSessionData(string key);
        void setSessionData(string key, NewUserDto userDto);
    }
}
