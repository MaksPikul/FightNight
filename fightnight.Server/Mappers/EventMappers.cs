using fightnight.Server.Dtos.User;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Enums;
using fightnight.Server.Models.Tables;
using fightnight.Server.Dtos.Event;

namespace fightnight.Server.Mappers
{

    public class EventDtoWCodes : EventResDto
    {
        public string joinCode { get; set; }
        //public string fighterJoinCode { get; set; }
    }




    public static class EventMappers
    {

        // I need to fix this somehow :/ cant be having two very similar mappers
        public static EventMembersDTO MapToEventMembersDTO(this Event eventModel, List<MemberResDto> userEventModel, EventRole role)
        {
            EventMembersDTO emDTO = new EventMembersDTO {
                Id = eventModel.Id,
                title = eventModel.title,
                startTime = eventModel.startTime,
                startDate = eventModel.date,
                //eventDur = eventModel.eventDur,
                venueAddress = eventModel.venueAddress,
                desc = eventModel.desc,
                eventType = eventModel.eventType,
                status = eventModel.status,
                organizer = eventModel.organizer,
                //adminId = eventModel.adminId,
                numberMatches = eventModel.numberMatches,
                numberRounds = eventModel.numberRounds,
                roundDuration = eventModel.roundDuration,
                role = role,

                EventMembers = userEventModel
            };

            return emDTO;
        }

        public static EventResDto ToEventDto(this Event eventModel, EventRole role)
        {
            return new EventResDto
            {
                Id = eventModel.Id,
                title = eventModel.title,
                startTime = eventModel.startTime,
                startDate = eventModel.date,
                //eventDur = eventModel.eventDur,
                venueAddress = eventModel.venueAddress,
                desc = eventModel.desc,
                eventType = eventModel.eventType,
                status = eventModel.status,
                organizer = eventModel.organizer,
                //adminId = eventModel.adminId,
                numberMatches = eventModel.numberMatches,
                numberRounds = eventModel.numberRounds,
                roundDuration = eventModel.roundDuration,
                role = role
            };
        }

        public static EventDtoWCodes ToEventDtoWCodes(this Event eventModel, EventRole role)
        {
            return new EventDtoWCodes
            {
                Id = eventModel.Id,
                title = eventModel.title,
                startTime = eventModel.startTime,
                startDate = eventModel.date,
                //eventDur = eventModel.eventDur,
                venueAddress = eventModel.venueAddress,
                desc = eventModel.desc,
                eventType = eventModel.eventType,
                status = eventModel.status,
                organizer = eventModel.organizer,
                //adminId = eventModel.adminId,
                numberMatches = eventModel.numberMatches,
                numberRounds = eventModel.numberRounds,
                roundDuration = eventModel.roundDuration,
                role = role,
                joinCode = eventModel.joinCode,
                //fighterJoinCode = eventModel.fighterJoinCode,
            };
        }


    }
}
