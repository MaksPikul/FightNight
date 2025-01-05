using fightnight.Server.Enums;

namespace fightnight.Server.Dtos.Member
{
    public class MemberResDto
    {
        public string Id {  get; set; }
        public string Email { get; set; } 
        public string Username { get; set; }
        public string picture { get; set; }
        public EventRole Role { get; set; }
        //AppUserEventId - the id of the entry, probably not good
        
    }
}
