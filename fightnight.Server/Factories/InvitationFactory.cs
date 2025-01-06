using fightnight.Server.Dtos.Member;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Factories
{
    public static class InvitationFactory
    {
        public static Invitation CreateInvite(InviteMemberReqDto sendInvBody)
        {
            Invitation invite = new Invitation
            {
                userEmail = sendInvBody.newMemberEmail,
                eventId = sendInvBody.eventId,
                //expiration = DateTime.UtcNow.AddDays(Expiration),
                proposedRole = sendInvBody.role,
            };

            return invite;
        }
    }
}
