using fightnight.Server.Dtos.User;
using fightnight.Server.models;

namespace fightnight.Server.Interfaces
{
    public interface IUserRepo
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateUserAsync(User userModel);
    }
}
