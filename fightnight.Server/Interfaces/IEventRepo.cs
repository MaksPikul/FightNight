using fightnight.Server.Dtos.User;
using fightnight.Server.models;
using fightnight.Server.Models;

namespace fightnight.Server.Interfaces
{
    public interface IEventRepo
    {
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event> CreateEventAsync(Event eventModel);
        Task<List<Event>> GetUserEvents(AppUser user);
        Task<Event> GetByEventIdAsync(string eventId);
        Task<AppUserEvent> CreateAppUserEventAsync(AppUserEvent userEvent);
    }
}
