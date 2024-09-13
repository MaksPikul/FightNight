using fightnight.Server.Data;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces;
using fightnight.Server.models;
using fightnight.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace fightnight.Server.repo
{
    public class EventRepo : IEventRepo
    {
        private readonly AppDBContext _context;
        public EventRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Event.ToListAsync();
        }
        public async Task<Event?> GetEventAsync(string id)
        {
            return await _context.Event.FindAsync(id);
        }

        public async Task<Event> CreateEventAsync(Event EventModel)
        {
            await _context.Event.AddAsync(EventModel);
            await _context.SaveChangesAsync();
            return EventModel;
        }

        public async Task<List<EventDto>> GetUserEvents(AppUser user)
        {
            return await _context.AppUserEvent.Where(u => u.AppUserId == user.Id)
                .Select(eventV => 
                    new EventDto
                    {
                    id = eventV.EventId,
                    title = eventV.Event.title,
                    date = eventV.Event.date,
                    adminId = eventV.Event.adminId,
                    role = eventV.Role,
                    venueAddress = eventV.Event.venueAddress,
                    time = eventV.Event.time,  
                    status = eventV.Event.status,
                    })
                .ToListAsync();
        }

        public async Task<AppUserEvent> CreateAppUserEventAsync(AppUserEvent userEvent)
        {
            await _context.AppUserEvent.AddAsync(userEvent);
            await _context.SaveChangesAsync();
            return userEvent;
        }   
        public EventRole GetUserEventRoleAsync(string userId, string eventId)
        {
            return _context.AppUserEvent.Where(u => u.EventId == eventId && u.AppUserId == userId).Select(u => u.Role).FirstOrDefault();
        }

    }
} 
