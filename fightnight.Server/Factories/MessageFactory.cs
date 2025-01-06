using fightnight.Server.Dtos.Message;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Factories
{
    public static class MessageFactory
    {
        public static Message CreateMessage(AddMessageBody msgBody, string userId)
        {
            Message newMsg = new Message
            {
                message = msgBody.msg,
                userId = userId,
                username = msgBody.username,
                picture = msgBody.picture,
                eventId = msgBody.eventId
            };

            return newMsg;
        }
    }
}
