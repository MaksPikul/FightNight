using fightnight.Server.Enums;

namespace fightnight.Server.Dtos.Member
{
    public class ReturnMemberDto
    {
        public string Id {  get; set; }
        public string Email { get; set; } 
        public string Username { get; set; }
        public string Picture { get; set; }
        public EventRole Role { get; set; }
        //AppUserEventId - the id of the entry, probably not good
        
    }
}
