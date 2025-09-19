using Microsoft.EntityFrameworkCore;
using TourAgencyManagement_API.Data;
using TourAgencyManagement_API.Models;
using TourAgencyManagement_API.Repositories.Interfaces;

namespace TourAgencyManagement_API.Repositories
{
    public class TourAgencyRepository : ITourAgencyRepository
    {
        private readonly TourAgencyContext _context;

        public TourAgencyRepository(TourAgencyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourAgency>> GetAllAsync()
        {
            return await _context.TourAgencies.AsNoTracking().ToListAsync();
        }

        public async Task<TourAgency?> GetByIdAsync(Guid id)
        {
            // FindAsync dùng key, có tracking (phù hợp Update/Delete)
            return await _context.TourAgencies.FindAsync(id);
        }

        public async Task<TourAgency> AddAsync(TourAgency agency)
        {
            _context.TourAgencies.Add(agency);
            await _context.SaveChangesAsync();
            return agency;
        }

        public async Task UpdateAsync(TourAgency agency)
        {
            _context.TourAgencies.Update(agency);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var agency = await _context.TourAgencies.FindAsync(id);
            if (agency != null)
            {
                _context.TourAgencies.Remove(agency);
                await _context.SaveChangesAsync();
            }
        }
    }
}