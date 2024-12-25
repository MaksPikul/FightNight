namespace fightnight.Server.Interfaces
{
    public interface IEmail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string GetBody();
    }
}
