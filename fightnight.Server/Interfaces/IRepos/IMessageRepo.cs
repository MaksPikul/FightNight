using fightnight.Server.Dtos.Message;
using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces.IRepos
{
    public interface IMessageRepo
    {
        //might need to be TResult
        Task<Message> CreateMessageAsync(Message msg);
        Task<Message> DeleteMessageAsync(Message msg); //Message msg);
        Task<List<ReturnMessageDto>> GetMessagesAsync(string eventId); //+, string userId); or , string groupId)
        Task<Message> UpdateMessageAsync(Message msg);
        Task<Message> GetMessageAsync(string msgId);


    }
}
