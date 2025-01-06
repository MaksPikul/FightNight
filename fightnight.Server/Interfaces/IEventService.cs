using fightnight.Server.Enums;

namespace fightnight.Server.Interfaces
{
    public interface IEventService
    {
        bool IsEventRoleValid(EventRole roleCheck, string userId, string eventId);
    }
}
