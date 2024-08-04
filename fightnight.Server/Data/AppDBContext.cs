using fightnight.Server.models;
using Microsoft.EntityFrameworkCore;

namespace fightnight.Server.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }

        public DbSet<User> User { get; set; } 

    }
}
