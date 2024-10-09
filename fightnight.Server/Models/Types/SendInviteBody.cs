using fightnight.Server.Enums;

namespace fightnight.Server.Models.Types
{
    public class SendInviteBody
    {
        public string newMemberEmail{ get; set; } = string.Empty;
        public string eventId { get; set; } = string.Empty;
        public EventRole role { get; set; }
    }
}
