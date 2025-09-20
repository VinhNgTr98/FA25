using Microsoft.EntityFrameworkCore;
using TourManagement.Data;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;

namespace TourManagement.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly TourContext _context;

        public TourRepository(TourContext context)
        {
            _context = context;
        }

        public IQueryable<Tour> GetAllAsync()
        {
            return _context.Tours.AsNoTracking().AsQueryable();
        }

        public async Task<Tour?> GetByIdAsync(Guid id)
        {
            return await _context.Tours.FindAsync(id);
        }

        public async Task AddAsync(Tour tour)
        {
            await _context.Tours.AddAsync(tour);
        }

        public void Update(Tour tour)
        {
            _context.Tours.Update(tour);
        }

        public void Delete(Tour tour)
        {
            _context.Tours.Remove(tour);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}