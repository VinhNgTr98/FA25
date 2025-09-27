using CarManagement_API.Data;
using CarManagement_API.Models;
using CarManagement_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarManagement_API.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarManagement_APIContext _context;

        public CarRepository(CarManagement_APIContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Car.ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(Guid carId)
        {
            return await _context.Car.FindAsync(carId);
        }

        public async Task AddAsync(Car car)
        {
            await _context.Car.AddAsync(car);
        }

        public async Task UpdateAsync(Car car)
        {
            _context.Car.Update(car);
        }

        public async Task DeleteAsync(Guid carId)
        {
            var car = await GetByIdAsync(carId);
            if (car != null)
            {
                _context.Car.Remove(car);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Car>> GetFilteredCarsAsync(
            string? transmission = null,
            string? fuel = null,
            string? carBrand = null,
            string? carName = null,
            int? engineCc = null)
        {
            var query = _context.Car.AsQueryable();

            if (!string.IsNullOrEmpty(transmission))
                query = query.Where(c => c.Gear.Contains(transmission));

            if (!string.IsNullOrEmpty(fuel))
                query = query.Where(c => c.Engine.Contains(fuel));

            if (!string.IsNullOrEmpty(carBrand))
                query = query.Where(c => c.CarBrand.Contains(carBrand));

            if (!string.IsNullOrEmpty(carName))
                query = query.Where(c => c.CarName.Contains(carName));

            return await query.ToListAsync();
        }
        public async Task<List<Car>> GetByVehicleAgencyIdAsync(Guid vehicleAgencyId)
        {
            return await _context.Car
                .Where(c => c.VehicleAgencyId == vehicleAgencyId)
                .ToListAsync();
        }
    }
}
