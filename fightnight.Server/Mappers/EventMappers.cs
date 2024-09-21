using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Mappers
{
    public static class EventMappers
    {
        public static EventDto ToEventDto(this Event eventModel, EventRole role)
        {
            return new EventDto
            {
                id = eventModel.id,
                title = eventModel.title,
                time = eventModel.time,
                date = eventModel.date,
                eventDur = eventModel.eventDur,
                venueAddress = eventModel.venueAddress,
                desc = eventModel.desc,
                type = eventModel.type,
                status = eventModel.status,
                organizer = eventModel.organizer,
                adminId = eventModel.adminId,
                numMatches = eventModel.numMatches,
                numRounds = eventModel.numRounds,
                roundDur = eventModel.roundDur,
                role = role
            };
        }

        public static Event EventFromCreateDto(this CreateEventDto eventDto)
        {
            return new Event
            {
                title = eventDto.Title,
                date = eventDto.Date,

            };
        }
    }
}
