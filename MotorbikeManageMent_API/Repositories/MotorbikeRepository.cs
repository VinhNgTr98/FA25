using Microsoft.EntityFrameworkCore;
using MotorbikeManageMent_API.Data;
using MotorbikeManageMent_API.Models;
using MotorbikeManageMent_API.Repositories.Interfaces;

namespace MotorbikeManageMent_API.Repositories
{
    public class MotorbikeRepository : IMotorbikeRepository
    {
        private readonly MotorbikeContext _context;

        public MotorbikeRepository(MotorbikeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Motorbike>> GetAllAsync()
        {
            return await _context.Motorbike.ToListAsync();
        }

        public async Task<Motorbike?> GetByIdAsync(Guid id)
        {
            return await _context.Motorbike.FindAsync(id);
        }

        public async Task AddAsync(Motorbike motorbike)
        {
            await _context.Motorbike.AddAsync(motorbike);
        }

        public async Task UpdateAsync(Motorbike motorbike)
        {
            _context.Motorbike.Update(motorbike);
        }

        public async Task DeleteAsync(Guid id)
        {
            var motorbike = await GetByIdAsync(id);
            if (motorbike != null)
            {
                _context.Motorbike.Remove(motorbike);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Phương thức lọc các xe theo các tiêu chí
        public async Task<List<Motorbike>> GetFilteredMotorbikesAsync(
            string? transmission = null,
            string? fuel = null,
            string? motorbikeBrand = null,
            string? motorbikeName = null,
            int? engineCc = null)
        {
            var query = _context.Motorbike.AsQueryable();

            // Lọc theo Transmission
            if (!string.IsNullOrEmpty(transmission))
                query = query.Where(m => m.Transmission.Contains(transmission));

            // Lọc theo Fuel
            if (!string.IsNullOrEmpty(fuel))
                query = query.Where(m => m.Fuel.Contains(fuel));

            // Lọc theo MotorbikeBrand
            if (!string.IsNullOrEmpty(motorbikeBrand))
                query = query.Where(m => m.MotorbikeBrand.Contains(motorbikeBrand));

            // Lọc theo MotorbikeName
            if (!string.IsNullOrEmpty(motorbikeName))
                query = query.Where(m => m.MotorbikeName.Contains(motorbikeName));

            // Lọc theo EngineCc
            if (engineCc.HasValue)
                query = query.Where(m => m.EngineCC == engineCc.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Motorbike>> GetByVehicleAgencyIdAsync(Guid vehicleAgencyId)
        {
            return await _context.Motorbike
                .Where(c => c.VehicleAgencyId == vehicleAgencyId)
                .ToListAsync();
        }
    }
}
