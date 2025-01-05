namespace fightnight.Server.Dtos.Account
{
    public class AuthedUserResDto
    {
        public string UserId {  get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public bool IsAuthed { get; set; } = false;
        public string Role { get; set; } = string.Empty;
    }
}
