using System.ComponentModel.DataAnnotations;

namespace fightnight.Server.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string? Username {  get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; } 
        [Required]
        public string? Password {  get; set; }

        public string? inviteId { get; set; } = string.Empty;
    }
}
