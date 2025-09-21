using Microsoft.EntityFrameworkCore;
using TourManagement.Data;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;

namespace TourManagement.Repositories
{
    public class ItineraryRepository : IItineraryRepository
    {
        private readonly TourContext _context;

        public ItineraryRepository(TourContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Itinerary>> GetAllAsync()
        {
            return await _context.Itineraries.ToListAsync();
        }

        public async Task<Itinerary?> GetByIdAsync(Guid id)
        {
            return await _context.Itineraries.FindAsync(id);
        }

        public async Task<IEnumerable<Itinerary>> GetByTourIdAsync(Guid tourId)
        {
            return await _context.Itineraries
                .Where(i => i.TourID == tourId)
                .OrderBy(i => i.ItineraryOrder)
                .ToListAsync();
        }

        public async Task<Itinerary> CreateAsync(Itinerary itinerary)
        {
            _context.Itineraries.Add(itinerary);
            await _context.SaveChangesAsync();
            return itinerary;
        }

        public async Task<bool> UpdateAsync(Itinerary itinerary)
        {
            _context.Itineraries.Update(itinerary);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Itineraries.FindAsync(id);
            if (entity == null) return false;
            _context.Itineraries.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
