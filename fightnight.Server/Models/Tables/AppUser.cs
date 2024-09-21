using Microsoft.AspNetCore.Identity;

namespace fightnight.Server.Models.Tables
{
    public class AppUser : IdentityUser
    {
        public List<AppUserEvent> AppUserEvents { get; set; } = new List<AppUserEvent>();

    }
}
