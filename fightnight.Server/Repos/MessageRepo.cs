using fightnight.Server.Data;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Mvc;

namespace fightnight.Server.Repos
{
    public class MessageRepo : IMessageRepo
    {
        private readonly AppDBContext _context;
        public MessageRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Message> CreateMessage(Message msg)
        {
            await _context.Message.AddAsync(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        public async Task<Message> DeleteMessage(Message msg)
        {
            _context.Message.Remove(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        public List<Message> GetMessages(string eventId)
        {
            return _context.Message.Where(u => u.id == eventId).ToList();
        }

        //TResult
        public async Task<Message> UpdateMessage(Message msg)
        {
            _context.Message.Update(msg);
            await _context.SaveChangesAsync();
            return msg;
        }
    }
}
