using fightnight.Server.Data;
using fightnight.Server.Dtos.Member;
using fightnight.Server.Dtos.User;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Mappers;
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

        public async Task<MemberResDto> GetEventMemberProfileAsync(string email, string eventId)
        {
            MemberResDto member =  await _context.AppUserEvent
                .Include(e => e.AppUser)
                .Where(u => u.Event.Id == eventId && u.AppUser.NormalizedEmail == email.ToUpper())
                .Select(ue => ue.AppUser.MapToMemberResDto()).FirstOrDefaultAsync();

            return member;
        }

        public async Task<AppUserEvent> RemoveMemberFromEventAsync(AppUserEvent aue)
        {
            // eventId,Member key
            // Remvove this entry,
            // WHen fetching again, it will populate new entry

            _context.AppUserEvent.Remove(aue);
            await _context.SaveChangesAsync();
            return aue;
        }
        
    }
}
