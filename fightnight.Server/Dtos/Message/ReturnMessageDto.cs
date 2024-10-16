namespace fightnight.Server.Dtos.Message
{
    public class ReturnMessageDto
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string eventId { get; set; } = string.Empty;
        public string userId { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string picture {  get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public DateTime timeStamp { get; set; } = DateTime.Now;
        public bool IsEdited { get; set; } = false;
    }
}
