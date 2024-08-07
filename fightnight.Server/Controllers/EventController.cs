using fightnight.Server.Data;
using fightnight.Server.Dtos.User;
using fightnight.Server.Extensions;
using fightnight.Server.Interfaces;
using fightnight.Server.Mappers;
using fightnight.Server.models;
using fightnight.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
                
            var eventsDto = events.Select(s => s.ToEventDto());

            return Ok(events); 
        }


        // Gets Events related to the user an
        [HttpGet("user/{id}")]
        [Authorize]
        public async Task <IActionResult> GetEventsByUserId([FromRoute] string id)
        {
            //var email = User.GetEmail();
            //FindByEmailAsync(email);
            var appUser = await _userManager.FindByIdAsync(id);
            var userEvents = await _eventRepo.GetUserEvents(appUser);
            return Ok(userEvents);
        }

        // Once redirected, get info about the event
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var eventV = await _eventRepo.GetByIdAsync(id);
            
            if (eventV == null) {
                return NotFound();
            }

            return Ok(eventV.ToEventDto());
        }

        //Creates event, Adds to Joined Table, Returns Event Id to redirect
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventDto){


            var eventModel = eventDto.EventFromCreateDto();
            await _eventRepo.CreateEventAsync(eventModel);

            var email = User.GetEmail();
            
            var appUser = await _userManager.FindByEmailAsync(email);
            
            var userEvents = await _eventRepo.GetUserEvents(appUser);
            if (userEvents.Any(e => e.id == eventModel.id)) return BadRequest("Cannot add same event");
            

            var AppUserEvent = new AppUserEvent
            {
                EventId = eventModel.id,
                AppUserId = appUser.Id,
                // add role
            };

            await _eventRepo.CreateAppUserEventAsync(AppUserEvent);
            if (AppUserEvent == null) return StatusCode(500, "Could not add AppUserEvent to DB");


            return Ok(eventModel.id);

            /*
            return CreatedAtAction(
                nameof(GetById), 
                new { id = eventModel.id }
                //eventModel.ToUserDto()
             );
            */
        }

        //Create entry to join table, prob not neccessary
        /*
        [HttpPost("user/{id}")]
        [Authorize]
        public async Task<IActionResult> AddUserEvents(string eventId)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);
            var eventV = await _eventRepo.GetByEventIdAsync(eventId);

            if (eventV == null) return BadRequest("Event not found");

            var userEvents = await _eventRepo.GetUserEvents(appUser);

            if (userEvents.Any(e => e.id == eventId)) return BadRequest("Cannot add same event");

            var AppUserEvent = new AppUserEvent
            {
                EventId = eventV.id,
                AppUserId = appUser.Id,
            };

            await _eventRepo.CreateAppUserEventAsync(AppUserEvent);

            if (AppUserEventModel == null) return StatusCode(500, "Could not add AppUserEvent to DB");

            return Created();
        }
        */
    }
}
