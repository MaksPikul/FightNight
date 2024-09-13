using fightnight.Server.Enums;

namespace fightnight.Server.Dtos.User
{
    public class EventDto
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string title { get; set; } = string.Empty;
        public DateTime date { get; set; } = DateTime.Now;
        public string time {  get; set; } = string.Empty;
        public TimeSpan eventDur { get; set; } = TimeSpan.Zero;
        public string venue { get; set; } 
        public string venueAddress { get; set; } 
        public string desc { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public EventStatus status { get; set; } = EventStatus.Planning;
        public string organizer { get; set; } = string.Empty;
        public string adminId { get; set; } = string.Empty;
        public int numMatches { get; set; } = 3;
        public int numRounds { get; set; } = 3;
        public int roundDur { get; set; } = 3;
        public EventRole role { get; set; }

    }
}
