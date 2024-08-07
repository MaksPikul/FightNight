using Microsoft.AspNetCore.Identity;

namespace fightnight.Server.Models
{
    public class AppUser : IdentityUser{
        public List<AppUserEvent> AppUserEvents { get; set; } = new List<AppUserEvent>();
        
    }
}
