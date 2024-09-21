using fightnight.Server.Enums;

namespace fightnight.Server.Dtos.NewFolder
{
    public class NewMemberDto
    {
        public string eventId { get; set; }
        public string newMemberId { get; set; }
        public EventRole newRole { get; set; }  
    }
}
