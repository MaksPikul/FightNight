using fightnight.Server.Dtos.Member;
using fightnight.Server.Enums;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Mappers
{
    public static class MemberMappers
    {
        public static MemberResDto MapToMemberResDto(this AppUser user)
        {
            return new MemberResDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                picture = user.Picture,
                //Role = role
            };
        }
    }
}
