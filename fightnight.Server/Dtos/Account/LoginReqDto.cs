using System.ComponentModel.DataAnnotations;

namespace fightnight.Server.Dtos.Account
{
    public class LoginReqDto
    {
        [Required]
        [EmailAddress]
        public string Email {  get; set; }
        [Required]
        public string Password { get; set; }
        public bool rememberMe { get; set; }
        public string inviteId { get; set; } = string.Empty;
    }
}
