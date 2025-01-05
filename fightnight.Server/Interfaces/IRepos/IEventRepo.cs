using fightnight.Server.Dtos.Event;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces.IRepos
{
    public interface IEventRepo
    {
        Task<List<Event>> GetAllAsync();
        Task<Event> GetEventONLYAsync(string id);
        Task<EventMembersDTO> GetEventWITHMembersAsync(string id);
        Task<Event> GetEventWithJoinCodeAsync(string code);
        Task<Event> CreateEventAsync(Event eventModel);
        Task<List<EventResDto>> GetUserEvents(string userId);
        Task<Event> DeleteEventAsync(Event eventVar);
        Task<Event> UpdateEventAsync(Event eventVar);
        EventRole GetUserEventRole(string userId, string eventId);
    }
}
