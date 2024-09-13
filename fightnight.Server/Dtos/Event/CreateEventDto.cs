using fightnight.Server.Enums;
using fightnight.Server.Models;

namespace fightnight.Server.Dtos.User
{
    public class CreateEventDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        
    }
}
