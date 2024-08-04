using System.ComponentModel.DataAnnotations;

namespace fightnight.Server.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string? Username {  get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string? Password {  get; set; }
    }
}
