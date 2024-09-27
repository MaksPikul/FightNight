using Microsoft.AspNetCore.Identity;

namespace fightnight.Server.Models.Tables
{
    public class AppUser : IdentityUser
    {
        public List<AppUserEvent> AppUserEvents { get; set; } = new List<AppUserEvent>();
        public List<Message> Messages { get; set; } = new List<Message>();
        public List<Invitation> Invitations { get; set; } = new List<Invitation>();

    }
}
