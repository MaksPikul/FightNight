using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace fightnight.Server.Data
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }

        public DbSet<Event> Event { get; set; }
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<AppUserEvent> AppUserEvent { get; set; }
        public DbSet<Invitation> Invitation { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUserEvent>(x => x.HasKey(p => new { p.AppUserId, p.EventId }));
            builder.Entity<AppUserEvent>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.AppUserEvents)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<AppUserEvent>()
                .HasOne(u => u.Event)
                .WithMany(u => u. AppUserEvents)
                .HasForeignKey(p => p.EventId);

            builder.Entity<Message>(x => x.HasKey(p => new { p.id }));
            builder.Entity<Message>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Messages)
                .HasForeignKey(p => p.userId);

            builder.Entity<Message>()
                .HasOne(u => u.Event)
                .WithMany(u => u.Messages)
                .HasForeignKey(p => p.eventId);

            builder.Entity<Invitation>(x => x.HasKey(p => new { p.userId, p.eventId }));
            builder.Entity<Invitation>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Invitations)
                .HasForeignKey(p => p.userId);

            builder.Entity<Invitation>()
                .HasOne(u => u.Event)
                .WithMany(u => u.Invitations)
                .HasForeignKey(p => p.eventId);

            List<IdentityRole> roles = new List<IdentityRole> {
                new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole
                {
                    Name = "user",
                    NormalizedName = "USER",
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
