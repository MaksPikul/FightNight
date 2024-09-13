using fightnight.Server.Enums;
using fightnight.Server.models;

namespace fightnight.Server.Models
{
    public class AppUserEvent
    {
        public string AppUserId { get; set; }
        public string  EventId { get; set; }
        public AppUser AppUser { get; set; }
        public Event Event { get; set; }
        public EventRole Role { get; set; }
    }
}
