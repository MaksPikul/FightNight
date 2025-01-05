using fightnight.Server.Enums;

namespace fightnight.Server.Dtos.Member
{
    public class InviteMemberReqDto
    {
        public string newMemberEmail { get; set; } = string.Empty;
        public string eventId { get; set; } = string.Empty;
        public EventRole role { get; set; }
    }
}
