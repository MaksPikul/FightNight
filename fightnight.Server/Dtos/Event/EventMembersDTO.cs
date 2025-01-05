using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.User;

namespace fightnight.Server.Dtos.Event
{
    public record EventMembersDTO : EventResDto
    {
        public List<MemberResDto> EventMembers { get; set; } = new List<MemberResDto>();
    }
}
