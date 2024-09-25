using fightnight.Server.Data;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace fightnight.Server.Repos
{
    public class MemberRepo : IMemberRepo
    {
        private readonly AppDBContext _context;
        public MemberRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task<AppUserEvent> AddMemberToEventAsync(AppUserEvent userEvent)
        {
            await _context.AppUserEvent.AddAsync(userEvent);
            await _context.SaveChangesAsync();
            return userEvent;
        }

        public async Task<List<ReturnMemberDto>> GetEventMembersAsync(string eventId)
        {
            return await _context.AppUserEvent.Where(u => u.EventId == eventId )
                .Select(eventV =>
                    new ReturnMemberDto
                    {
                        Id = eventV.AppUser.Id,
                        Username = eventV.AppUser.UserName,
                        Email = eventV.AppUser.Email,
                        Picture = "add to column DB", //eventV.AppUser.Picture
                        Role = eventV.Role,
                    })
                .ToListAsync();
        }

        public async Task<AppUserEvent> GetMemberAsync(string userId, string eventId)
        {
            return await _context.AppUserEvent.Where(u => u.EventId == eventId && u.AppUserId ==userId).FirstOrDefaultAsync();
        }

        public async Task<AppUserEvent> RemoveMemberFromEventAsync(AppUserEvent aue)
        {
            _context.AppUserEvent.Remove(aue);
            await _context.SaveChangesAsync();
            return aue;
        }

        public async Task<Boolean> CheckIfMemberAsync(string userId, string eventId)
        {
            var member = await _context.AppUserEvent.Where(u => u.EventId == eventId && u.AppUserId == userId).FirstOrDefaultAsync();
            return member != null;
        }
        
    }
}
