using Microsoft.EntityFrameworkCore;
using TourManagement.Data;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;

namespace TourManagement.Repositories
{
    public class TourMemberRepository : ITourMemberRepository
    {
        private readonly TourContext _context;
        public TourMemberRepository(TourContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourMember>> GetAllAsync()
        {
            return await _context.TourMembers.ToListAsync();
        }

        public async Task<TourMember?> GetByIdAsync(Guid id)
        {
            return await _context.TourMembers.FindAsync(id);
        }

        public async Task<TourMember> AddAsync(TourMember member)
        {
            _context.TourMembers.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<bool> UpdateAsync(TourMember member)
        {
            _context.TourMembers.Update(member);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var member = await _context.TourMembers.FindAsync(id);
            if (member == null) return false;

            _context.TourMembers.Remove(member);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
