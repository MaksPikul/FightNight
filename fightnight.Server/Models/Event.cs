using fightnight.Server.Enums;
using fightnight.Server.Models;

namespace fightnight.Server.models
{
    public class Event
    {
        //normalise by taking venue and organiser 

        public string id { get; set; } = Guid.NewGuid().ToString();
        public string title { get; set; } = string.Empty;
        public DateTime dateTime { get; set; }
        public TimeSpan eventDur { get; set; }
        public string venue { get; set; } = string.Empty;
        public string VenueAddress { get; set; } = string.Empty;
        public string desc {get; set;} = string.Empty;
        public string type { get; set; } = string.Empty;
        public EventStatus status { get; set; } = EventStatus.Planning;
        public string organizer { get; set; } = string.Empty;
        public string adminId { get; set; } = string.Empty;
        public int numRounds { get; set; } = 3;
        public int roundDur { get; set; } = 3;
        //participant list
        //mod list
        //img

        public List<AppUserEvent> AppUserEvents { get; set; } = new List<AppUserEvent>();

    }
}
