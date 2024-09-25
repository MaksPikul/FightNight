using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.Message;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Mappers
{
    public static class MessageMapper
    {
        public static ReturnMessageDto ReturnMessageMapper (this Message msgModel)
        {
            return new ReturnMessageDto {
                id = msgModel.id,
                userId = msgModel.userId,
                username = msgModel.username,
                //userPicture = msgModel.userPicture,
                eventId = msgModel.eventId,
                message = msgModel.message,
                IsEdited = msgModel.IsEdited,
                timeStamp = msgModel.timeStamp,
            };
        }
    }
}
