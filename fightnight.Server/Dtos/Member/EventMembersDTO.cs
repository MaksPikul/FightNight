using fightnight.Server.Dtos.User;

namespace fightnight.Server.Dtos.Member
{
    public record EventMembersDTO : EventDto
    {
        public List<MemberResDto> EventMembers { get; set; } = new List<MemberResDto>();
    }
}
