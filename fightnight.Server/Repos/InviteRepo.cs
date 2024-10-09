using fightnight.Server.Data;
using fightnight.Server.Models.Tables;
using fightnight.Server.Interfaces.IRepos;
using Microsoft.EntityFrameworkCore;

namespace fightnight.Server.Repos
{
    public class InviteRepo : IInviteRepo
    {
        private readonly AppDBContext _context;
        public InviteRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Invitation> AddInviteAsync(Invitation invite)
        {
            await _context.Invitation.AddAsync(invite);
            await _context.SaveChangesAsync();
            return invite;
        }

        public async Task<Invitation> DeleteInviteAsync(Invitation invitation)
        {
            throw new NotImplementedException();
        }

        public async Task<Invitation> GetInvitationAsync(string Id)
        {
            return await _context.Invitation.FindAsync(Id);
        }

        public async Task<Boolean> InviteExistsAsync(string Email)
        {
            var result = await _context.Invitation.Where(inv => inv.userEmail == Email).FirstOrDefaultAsync();
            return result != null;
        }

        public async Task<Invitation> UpdateInviteAsync(Invitation invite)
        {
            _context.Invitation.Update(invite);
            await _context.SaveChangesAsync();
            return invite;
        }
    }
}
