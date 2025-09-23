using Microsoft.EntityFrameworkCore;
using TourManagement.Data;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;

namespace TourManagement.Repositories
{
    public class TourGuideRepository : ITourGuideRepository
    {
        private readonly TourContext _context;
        public TourGuideRepository(TourContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourGuide>> GetAllAsync()
        {
            return await _context.TourGuides.ToListAsync();
        }

        public async Task<TourGuide?> GetByIdAsync(Guid id)
        {
            return await _context.TourGuides.FindAsync(id);
        }

        public async Task<TourGuide> AddAsync(TourGuide guide)
        {
            _context.TourGuides.Add(guide);
            await _context.SaveChangesAsync();
            return guide;
        }

        public async Task<bool> UpdateAsync(TourGuide guide)
        {
            _context.TourGuides.Update(guide);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var guide = await _context.TourGuides.FindAsync(id);
            if (guide == null) return false;

            _context.TourGuides.Remove(guide);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
