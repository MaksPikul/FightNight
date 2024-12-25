using fightnight.Server.Dtos.Account;

namespace fightnight.Server.Interfaces
{
    public interface IUserRegistrationService
    {
        Task<string> RegisterUserAsync(RegisterDto registerDto, HttpResponse response);
    }
}
