using fightnight.Server.Data;
using fightnight.Server.Dtos.Message;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Mappers;
using fightnight.Server.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace fightnight.Server.Repos
{
    public class MessageRepo : IMessageRepo
    {
        private readonly AppDBContext _context;

        public MessageRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Models.Tables.Message> CreateMessageAsync(Models.Tables.Message msg)
        {
            await _context.Message.AddAsync(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        public async Task<Models.Tables.Message> DeleteMessageAsync(Models.Tables.Message msg)
        {
            _context.Message.Remove(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        public async Task<Models.Tables.Message> GetMessageAsync(string msgId)
        {
            return await _context.Message.FindAsync(msgId);
        }

        public async Task<List<ReturnMessageDto>> GetMessagesAsync(string eventId, int offset, int limit)
        {
            return await _context.Message
               .Where(e => e.eventId == eventId)
               .OrderByDescending(m => m.timeStamp)
               .Skip(offset * limit)
               .Take(limit)
               .Select(m => m.ReturnMessageMapper())
               .Reverse()
               .ToListAsync();


            //return messages.OrderBy(m => m.timeStamp);
        }

        //TResult
        public async Task<Models.Tables.Message> UpdateMessageAsync(Models.Tables.Message msg)
        {
            _context.Message.Update(msg);
            await _context.SaveChangesAsync();
            return msg;
        }
    }
}
