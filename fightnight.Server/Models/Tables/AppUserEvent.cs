using fightnight.Server.Enums;

namespace fightnight.Server.Models.Tables
{
    public class AppUserEvent
    {
        public string AppUserId { get; set; }
        public string EventId { get; set; }
        public AppUser AppUser { get; set; }
        public Event Event { get; set; }
        public EventRole Role { get; set; }
    }
}
