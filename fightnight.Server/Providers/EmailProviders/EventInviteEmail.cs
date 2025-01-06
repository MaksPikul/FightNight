using fightnight.Server.Abstracts;

namespace fightnight.Server.Providers.EmailProviders
{
    public class EventInviteEmail : Email
    {
        public EventInviteEmail(string email, string inviteId) {

            Recipient = email;
            Subject = "You Have been Invited to Moderate a Fight Event!";

            string link = "https://localhost:5173/eventInvite?token=" + inviteId + "&email=" + email;

            Body = "Heres the link, " + link;
        }
    }
}
