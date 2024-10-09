using fightnight.Server.Enums;

namespace fightnight.Server.Models.Tables
{
    public class Event
    {
        //normalise by taking venue and organiser 

        public string id { get; set; } = Guid.NewGuid().ToString();
        public string title { get; set; } = string.Empty;
        public DateTime date { get; set; } = DateTime.Now;
        public TimeSpan eventDur { get; set; } = TimeSpan.Zero;
        public string time { get; set; } = string.Empty;
        public string venue { get; set; } = string.Empty;
        public string venueAddress { get; set; } = string.Empty;
        public string desc { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public EventStatus status { get; set; } = EventStatus.Planning;
        public string joinCode { get; set; } = Guid.NewGuid().ToString();
        //public string fighterJoinCode { get; set; } = string.Empty;
        public string organizer { get; set; } = string.Empty;
        public int numMatches { get; set; } = 3;
        public int numRounds { get; set; } = 3;
        public int roundDur { get; set; } = 3;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; } 
        //participant list
        //mod list
        public string bannerUrl { get; set; } = string.Empty;
        public List<AppUserEvent> AppUserEvents { get; set; } = new List<AppUserEvent>();
        public List<Message> Messages { get; set; } = new List<Message>();
        public List<Invitation> Invitations { get; set; } = new List<Invitation>();

    }

}
