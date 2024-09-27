using fightnight.Server.Data;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.Message;
using fightnight.Server.Dtos.User;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Mappers;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace fightnight.Server.Repos
{
    public class MessageRepo : IMessageRepo
    {
        private readonly AppDBContext _context;
        public MessageRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Message> CreateMessageAsync(Message msg)
        {
            await _context.Message.AddAsync(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        public async Task<Message> DeleteMessageAsync(Message msg)
        {
            _context.Message.Remove(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        public async Task<Message> GetMessageAsync(string msgId)
        {
            return await _context.Message.FindAsync(msgId);
        }

        public async Task<List<ReturnMessageDto>> GetMessagesAsync(string eventId)
        {
            return await _context.Message
                .Where(e => e.eventId == eventId)
                .OrderBy(m => m.timeStamp)
                .Select(m => m.ReturnMessageMapper())
                .ToListAsync();

            //return messages.OrderBy(m => m.timeStamp);
        }   

        //TResult
        public async Task<Message> UpdateMessageAsync(Message msg)
        {
            _context.Message.Update(msg);
            await _context.SaveChangesAsync();
            return msg;
        }
    }
}
