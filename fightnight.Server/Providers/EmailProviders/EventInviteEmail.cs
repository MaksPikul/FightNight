using fightnight.Server.Abstracts;

namespace fightnight.Server.Providers.EmailProviders
{
    public class EventInviteEmail : Email
    {
        public EventInviteEmail(string email, string link) {

            Recipient = email;
            Subject = "You Have been Invited to Moderate a Fight Event!";

            Body = "Heres the link,";
        }
    }
}
