using fightnight.Server.Models.Tables;

namespace fightnight.Server.Interfaces.IRepos
{
    public interface IMessageRepo
    {
        //might need to be TResult
        Task<Message> CreateMessage(Message msg);
        Task<Message> DeleteMessage(Message msg); //Message msg);
        List<Message> GetMessages(string eventId); //+, string userId); or , string groupId)
        Task<Message> UpdateMessage(Message msg);
        
    }
}
