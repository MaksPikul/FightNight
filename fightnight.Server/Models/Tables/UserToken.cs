using fightnight.Server.Enums;

namespace fightnight.Server.Models.Tables
{
    public class UserToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string userId { get; set; }
        public string token { get; set; }
        public DateTime expiry { get; set; }
        public TokenType tokenType { get; set; }
        public string userEmail { get; set; }
        public AppUser appUser { get; set; }
    }
}
