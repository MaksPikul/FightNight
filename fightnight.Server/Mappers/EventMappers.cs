using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.models;

namespace fightnight.Server.Mappers
{
    public static class EventMappers
    {
        public static EventDto ToEventDto(this Event eventModel)
        {
            return new EventDto
            {
                id = eventModel.id,
                title = eventModel.title,
                dateTime = eventModel.dateTime,
                eventDur = eventModel.eventDur,
                venue = eventModel.venue,
                VenueAddress = eventModel.VenueAddress,
                desc = eventModel.desc,
                type = eventModel.type,
                status = eventModel.status,
                organizer = eventModel.organizer,
                numRounds = eventModel.numRounds,
                roundDur = eventModel.roundDur,

            };
        }

        public static Event EventFromCreateDto(this CreateEventDto eventDto)
        {
            return new Event
            {
                title = eventDto.title,
                dateTime = eventDto.dateTime,
                eventDur = eventDto.eventDur,
                venue = eventDto.venue,
                VenueAddress = eventDto.VenueAddress,
                desc = eventDto.desc,
                type = eventDto.type,
                status = eventDto.status,
                organizer = eventDto.organizer,
                adminId = eventDto.adminId,
                numRounds = eventDto.numRounds,
                roundDur = eventDto.roundDur,


            };
        }
    }
}
