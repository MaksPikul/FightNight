using fightnight.Server.Data;
using fightnight.Server.Dtos.Event;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Extensions;
using fightnight.Server.Interfaces;
using fightnight.Server.Mappers;
using fightnight.Server.models;
using fightnight.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fightnight.Server.Controllers
{
    [Route("/api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IEventRepo _eventRepo;
        private readonly UserManager<AppUser> _userManager;
        public EventController(AppDBContext context, UserManager<AppUser> userManager, IEventRepo eventRepo)
        {
            _eventRepo = eventRepo;
            _context = context;
            _userManager = userManager;
        }

        // gets all created events, cant imagine needing this
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventRepo.GetAllAsync();

            var eventsDto = events.Select(s => s);

            return Ok(events);
        }

        // Gets Events related to the user an
        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetEventsByUserId([FromRoute] string id)
        {
            //var email = User.GetEmail();
            //FindByEmailAsync(email);
            var appUser = await _userManager.FindByIdAsync(id);
            var userEvents = await _eventRepo.GetUserEvents(appUser);
            return Ok(userEvents);
        }

        // Once redirected, get info about the event
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);
            var userEventRole = _eventRepo.GetUserEventRoleAsync(appUser.Id, id);
            if (userEventRole == EventRole.Spectator)
            {
                return Unauthorized("Denied Access to Event Page");
            }

            var eventV = await _eventRepo.GetEventAsync(id);
            if (eventV == null)
            {
                return NotFound();
            }

            return Ok(eventV.ToEventDto(userEventRole));
        }

        //Creates event, Adds to Joined Table, Returns Event Id to redirect
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventDto)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);
            var eventModel = new Event
            {
                adminId = appUser.Id,
                title = eventDto.Title,
                date = eventDto.Date
            };
            //return Ok(eventDto.date);

            await _eventRepo.CreateEventAsync(eventModel);

            var userEvents = await _eventRepo.GetUserEvents(appUser);
            if (userEvents.Any(e => e.id == eventModel.id)) return BadRequest("Cannot add same event");

            var AppUserEvent = new AppUserEvent
            {
                EventId = eventModel.id,
                AppUserId = appUser.Id,
                Role = EventRole.Admin,
            };

            await _eventRepo.CreateAppUserEventAsync(AppUserEvent);
            if (AppUserEvent == null) return StatusCode(500, "Could not add AppUserEvent to DB");

            return Ok(eventModel.id);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDto eventDto)
        {

            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var ueRole = _eventRepo.GetUserEventRoleAsync(appUser.Id, eventDto.id);

            if (ueRole == null)
            {
                return BadRequest("No Event Found");
            }

            if (ueRole.Equals(EventRole.Admin))
            {
                var eventVs = await _eventRepo.GetEventAsync(eventDto.id);

                eventVs.title = eventDto.title;
                eventVs.time = eventDto.time;
                eventVs.date = eventDto.date;
                eventVs.desc = eventDto.desc;
                eventVs.venueAddress = eventDto.venueAddress;
                eventVs.numMatches = eventDto.numMatches;
                eventVs.numRounds = eventDto.numRounds;
                eventVs.roundDur = eventDto.roundDur;

                _context.Event.Update(eventVs);
                _context.SaveChanges();
                return Ok(eventVs);
            }
            else
            {
                return BadRequest("Admin Action, You are Unauthorized");
            }
        }

        [HttpDelete("{eventId}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent([FromRoute] string eventId)
        {

            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var ueRole = _eventRepo.GetUserEventRoleAsync(appUser.Id, eventId);
            

            if (ueRole == null)
            {
                return BadRequest("No Event Found");
            }
            else if (ueRole.Equals(EventRole.Admin))
            {
                var eventVar = await _eventRepo.GetEventAsync(eventId);
                _context.Event.Remove(eventVar);
                _context.SaveChanges();

                return Ok("Event Deleted");
            }
            else
            {
                return BadRequest("Admin Action, You are Unauthorized");
            }
        }
    }
}
