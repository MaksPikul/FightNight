using fightnight.Server.Data;
using fightnight.Server.Dtos.Event;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Mappers;
using fightnight.Server.Models.Tables;
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

        public async Task<Event> GetEventONLYAsync(string id)
        {
            return await _context.Event.FindAsync(id);
        }

        public async Task<EventMembersDTO> GetEventWITHMembersAsync(string id)
        {
            var eventWithMembers = await _context.Event
                .Where(e => e.Id == id)
                .Include(e => e.AppUserEvents) 
                    .ThenInclude(ue => ue.AppUser) 
                .Select(e => e.MapToEventMembersDTO(
                    e.AppUserEvents.Select(ue => ue.AppUser.MapToMemberResDto()).ToList(),
                    e.AppUserEvents.Select(ape => ape.Role).FirstOrDefault()
                )).FirstOrDefaultAsync();

            return eventWithMembers;
        }

        public Task<Event> GetEventWithJoinCodeAsync(string code)
        {
            return null;
        }

        public async Task<Event> CreateEventAsync(Event EventModel)
        {
            await _context.Event.AddAsync(EventModel);
            await _context.SaveChangesAsync();
            return EventModel;
        }

        public async Task<List<EventResDto>> GetUserEvents(string userId)
        {
            return await _context.AppUserEvent
                .Where(u => u.AppUserId == userId)
                .Select(eventV =>
                    new EventResDto
                    {
                        Id = eventV.Event.Id,
                        title = eventV.Event.title,
                        startDate = eventV.Event.date,
                        //adminId = eventV.Event.adminId,
                        role = eventV.Role,
                        venueAddress = eventV.Event.venueAddress,
                        startTime = eventV.Event.startTime,
                        status = eventV.Event.status,
                    })
                .ToListAsync();
        }

        public async Task<Event> DeleteEventAsync(Event eventVar)
        {
            _context.Event.Remove(eventVar);
            await _context.SaveChangesAsync();
            return eventVar;
        }

        public async Task<Event> UpdateEventAsync(Event eventVar)
        {
            _context.Event.Update(eventVar);
            await _context.SaveChangesAsync();
            return eventVar;
        }

        public EventRole GetUserEventRole(string userId, string eventId)
        {
            return _context.AppUserEvent.Where(u => u.EventId == eventId && u.AppUserId == userId).Select(u => u.Role).FirstOrDefault();
        }

        

        



        
    }
} 
