using fightnight.Server.Data;
using fightnight.Server.Dtos.User;
using fightnight.Server.Interfaces;
using fightnight.Server.models;
using Microsoft.EntityFrameworkCore;

namespace fightnight.Server.repo
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDBContext _context;
        public UserRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.User.ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User userModel)
        {
            await _context.User.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

    }
}
