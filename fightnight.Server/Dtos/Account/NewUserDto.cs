namespace fightnight.Server.Dtos.Account
{
    public class NewUserDto
    {
        public string userId {  get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string picture { get; set; } = string.Empty;
        public bool isAuthed { get; set; }
        public string role { get; set; } = string.Empty;
    }
}
