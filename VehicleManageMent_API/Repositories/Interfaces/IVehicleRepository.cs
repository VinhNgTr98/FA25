using VehicleManageMent_API.Models;

namespace VehicleManageMent_API.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
