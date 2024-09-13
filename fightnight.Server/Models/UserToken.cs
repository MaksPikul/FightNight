namespace fightnight.Server.Models
{
    public class UserToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string userId { get; set; }
        public string token { get; set; }
        public DateTime expiry { get; set; }
        // public TokenType tokenType {get; set;}
        public AppUser appUser { get; set; }
    }
}
