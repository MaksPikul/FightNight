using fightnight.Server.Enums;
using fightnight.Server.Mappers;

namespace fightnight.Server.Dtos.User
{
    public record EventResDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string title { get; set; } = string.Empty;
        public DateTime startDate { get; set; } = DateTime.Now;
        public string startTime {  get; set; } = string.Empty;
        public TimeSpan eventDur { get; set; } = TimeSpan.Zero;
        public string venueName { get; set; } 
        public string venueAddress { get; set; } 
        public string desc { get; set; } = string.Empty;
        public string eventType { get; set; } = string.Empty;
        public EventStatus status { get; set; } = EventStatus.Planning;
        public string organizer { get; set; } = string.Empty;
        public string adminId { get; set; } = string.Empty;
        public int numberMatches { get; set; } = 3;
        public int numberRounds { get; set; } = 3;
        public int roundDuration { get; set; } = 3;
        public EventRole role { get; set; }
    }
}
