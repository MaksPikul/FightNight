using fightnight.Server.Dtos.Account;

namespace fightnight.Server.Interfaces.IServices
{
    public interface ISessionService
    {
        NewUserDto getSessionData(string key);
        void setSessionData(string key, NewUserDto userDto);
    }
}
