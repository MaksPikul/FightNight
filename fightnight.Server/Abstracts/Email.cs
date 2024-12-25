namespace fightnight.Server.Abstracts
{
    public abstract class Email
    {
        public string Recipient { get; set;} = String.Empty;
        public string Subject { get; protected set; }
        public string Body { get; protected set; }
    }
}
