using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces.IRepos
{
    public interface IEventRepo
    {
        Task<List<Event>> GetAllAsync();
        Task<Event> GetEventAsync(string id);
        Task<Event> CreateEventAsync(Event eventModel);
        Task<List<EventDto>> GetUserEvents(AppUser user);
        Task<Event> DeleteEventAsync(Event eventVar);
        Task<Event> UpdateEventAsync(Event eventVar);
        EventRole GetUserEventRole(string userId, string eventId);
    }
}
