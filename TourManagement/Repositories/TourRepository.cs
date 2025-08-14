using TourManagement.Data;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;
using TourManagement.Services.Interfaces;

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
            return _context.Tours.AsQueryable();
        }

        public async Task<Tour?> GetByIdAsync(Guid id)
        {
            return await _context.Tours.FindAsync(id);
        }

        public async Task AddAsync(Tour Tour)
        {
            await _context.Tours.AddAsync(Tour);
        }

        public void Update(Tour Tour)
        {
            _context.Tours.Update(Tour);
        }

        public void Delete(Tour Tour)
        {
            _context.Tours.Remove(Tour);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
