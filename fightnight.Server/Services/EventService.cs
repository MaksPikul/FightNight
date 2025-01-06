using fightnight.Server.Data;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Identity;

namespace fightnight.Server.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepo _eventRepo;
        
        public EventService(
            IEventRepo eventRepo
        )
        {
            _eventRepo = eventRepo;
        }


        public bool IsEventRoleValid(EventRole roleCheck, string userId, string eventId)
        {
            EventRole ueRole = _eventRepo.GetUserEventRole(userId, eventId);
            if (!ueRole.Equals(EventRole.Admin))
            {
                return false;
            }
            return true;
        }

      

    }
}
