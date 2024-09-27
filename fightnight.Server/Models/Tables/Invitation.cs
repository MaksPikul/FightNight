using fightnight.Server.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace fightnight.Server.Models.Tables
{
    public class Invitation
    {
        public string userId { get; set; } = string.Empty;
        public string eventId { get; set; } = string.Empty;
        public InviteStatus status { get; set; } = InviteStatus.Pending;
        public DateTime invitedAt { get; set; } = DateTime.UtcNow;
        public DateTime respondedAt { get; set; }
        public DateTime expiration { get; set; } = DateTime.UtcNow.AddDays(30);
        public EventRole proposedRole { get; set; } = EventRole.Moderator;
        public AppUser AppUser { get; set; }
        public Event Event { get; set; }
    }
}
