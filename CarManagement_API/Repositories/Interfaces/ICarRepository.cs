using CarManagement_API.Models;

namespace CarManagement_API.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(Guid carId);
        Task AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(Guid carId);
        Task<int> SaveChangesAsync();

        Task<List<Car>> GetFilteredCarsAsync(
            string? transmission = null,
            string? fuel = null,
            string? carBrand = null,
            string? carName = null,
            int? engineCc = null
        );
    }
}
