namespace fightnight.Server.Dtos.Event
{
    public class UpdateEventDto
    {
        public string id { get; set; }
        public string title { get; set; }
        public DateTime date { get; set; } 
        public string desc { get; set; }
        public string time { get; set; }
        public string venueAddress { get; set; }
        public int numMatches { get; set; }
        public int numRounds { get; set; } 
        public int roundDur { get; set; } 
    }
}
