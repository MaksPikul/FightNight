﻿using fightnight.Server.Data;
using fightnight.Server.Dtos.Event;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Extensions;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Mappers;
using fightnight.Server.Models;
using fightnight.Server.Models.Tables;
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
        private readonly IMemberRepo _memberRepo;
        private readonly UserManager<AppUser> _userManager;
        public EventController(
            AppDBContext context, 
            UserManager<AppUser> userManager, 
            IEventRepo eventRepo,
            IMemberRepo memberRepo)
        {
            _eventRepo = eventRepo;
            _memberRepo = memberRepo;
            _context = context;
            _userManager = userManager;
        }

        // gets all created events, cant imagine needing this
        

        // Gets Events related to the user an
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetEventsByUserId()
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var userEvents = await _eventRepo.GetUserEvents(appUser.Id);
            return Ok(userEvents);
        }

        // Once redirected, get info about the event and its members
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEventByEventId([FromRoute] string id)
        {
            try
            {
                var email = User.GetEmail();
                var appUser = await _userManager.FindByEmailAsync(email);

                EventMembersDTO e = await _eventRepo.GetEventWITHMembersAsync(id);
                if (e == null){
                    return NotFound();
                }

                if (e.role == EventRole.Spectator) {
                    return Unauthorized("Unauthorized for this Action");
                }

                return Ok(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Creates event, Adds to Joined Table, Returns Event Id to redirect
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventReqDto eventDto)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            // Process payments

            if (eventDto.StartDate < DateTime.UtcNow) {
                return BadRequest("Cant create events for the past");
            }

            var eventModel = new Event
            {
                //adminId = appUser.Id,
                title = eventDto.Title,
                date = eventDto.StartDate,
            };

            await _eventRepo.CreateEventAsync(eventModel);

            var userEvents = await _eventRepo.GetUserEvents(appUser.Id);
            if (userEvents.Any(e => e.Id == eventModel.Id)) return BadRequest("Cannot add same event");

            var AppUserEvent = new AppUserEvent
            {
                EventId = eventModel.Id,
                AppUserId = appUser.Id,
                Role = EventRole.Admin,
            };

            await _memberRepo.AddMemberToEventAsync(AppUserEvent);
            if (AppUserEvent == null) return StatusCode(500, "Could not add AppUserEvent to DB");

            return Ok(eventModel.Id);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDto eventDto)
        {

            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var ueRole = _eventRepo.GetUserEventRole(appUser.Id, eventDto.id);

            if (ueRole == null)
            {
                return BadRequest("No Event Found");
            }

            if (ueRole.Equals(EventRole.Admin))
            {
                var eventVs = await _eventRepo. GetEventONLYAsync(eventDto.id);

                eventVs.title = eventDto.title;
                eventVs.startTime = eventDto.time;
                eventVs.date = eventDto.date;
                eventVs.desc = eventDto.desc;
                //eventVs.updatedAt = DateTime.Now;
                eventVs.venueAddress = eventDto.venueAddress;
                eventVs.numberMatches = eventDto.numMatches;
                eventVs.numberRounds = eventDto.numRounds;
                eventVs.roundDuration = eventDto.roundDur;

                await _eventRepo.UpdateEventAsync(eventVs);
                return Ok(eventVs.ToEventDto(ueRole));
            }
            else
            {
                return Unauthorized("Admin Action, You are Unauthorized");
            }
        }

        [HttpDelete("{eventId}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent([FromRoute] string eventId)
        {

            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);

            var ueRole = _eventRepo.GetUserEventRole(appUser.Id, eventId);
            

            if (ueRole == null)
            {
                return BadRequest("No Event Found");
            }
            else if (ueRole.Equals(EventRole.Admin))
            {
                var eventVar = await _eventRepo.GetEventONLYAsync(eventId);

                await _eventRepo.DeleteEventAsync(eventVar);
                
                // set redirect in response

                return Ok("Event Deleted");
            }
            else
            {
                return Unauthorized("Admin Action, You are Unauthorized");
            }
        }

        [HttpPatch("generate-code")]
        [Authorize]
        public async Task<IActionResult> GenerateCode([FromBody] string eventId, EventRole forRole)
        {
            var email = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(email);
            var ueRole = _eventRepo.GetUserEventRole(appUser.Id, eventId);

            if (!ueRole.Equals(EventRole.Admin))
            {
                return Unauthorized("Admin Action, You are Unauthorized");
            }

            var curEvent = await _eventRepo.GetEventONLYAsync(eventId);
            if (curEvent == null) return BadRequest("Event Not Found");

            // generate code
            var newCode = Guid.NewGuid().ToString();
            curEvent.joinCode = newCode;

            await _eventRepo.UpdateEventAsync(curEvent);

            //check for event code update creation success

            return Ok(newCode);
        }
    }
}
