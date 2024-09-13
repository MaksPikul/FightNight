using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.models;
using fightnight.Server.Models;

namespace fightnight.Server.Interfaces
{
    public interface IEventRepo
    {
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetEventAsync(string id);
        Task<Event> CreateEventAsync(Event eventModel);
        Task<List<EventDto>> GetUserEvents(AppUser user);
        Task<AppUserEvent> CreateAppUserEventAsync(AppUserEvent userEvent);
        EventRole GetUserEventRoleAsync(string userId, string eventId);
    }
}
