using fightnight.Server.Data;
using fightnight.Server.Dtos.User;
using fightnight.Server.Interfaces;
using fightnight.Server.models;
using fightnight.Server.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Event.FindAsync(id);
        }

        public async Task<Event> CreateEventAsync(Event EventModel)
        {
            await _context.Event.AddAsync(EventModel);
            await _context.SaveChangesAsync();
            return EventModel;
        }

        public async Task<List<Event>> GetUserEvents(AppUser user)
        {
            return await _context.AppUserEvent.Where(u => u.AppUserId == user.Id)
                .Select(eventV => new Event
                {
                    id = eventV.EventId,
                    title = eventV.Event.title,
                    dateTime = eventV.Event.dateTime,
                })
                .ToListAsync();
        }

        public async Task<Event> GetByEventIdAsync(string eventId)
        {
            return await _context.Event.FirstOrDefaultAsync(eId => eId.id == eventId);
        }

        public async Task<AppUserEvent> CreateAppUserEventAsync(AppUserEvent userEvent)
        {
            await _context.AppUserEvent.AddAsync(userEvent);
            await _context.SaveChangesAsync();
            return userEvent;
        }   

    }
} 
