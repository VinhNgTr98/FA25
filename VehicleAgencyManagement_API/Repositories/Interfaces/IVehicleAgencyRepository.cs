using VehicleAgencyManagement_API.Models;

namespace VehicleAgencyManagement_API.Repositories.Interfaces
{
    public interface IVehicleAgencyRepository
    {
        Task<IEnumerable<VehicleAgency>> GetAllAsync();
        Task<VehicleAgency?> GetByIdAsync(Guid id);
        Task AddAsync(VehicleAgency vehicleAgency);
        Task UpdateAsync(VehicleAgency vehicleAgency);
        Task DeleteAsync(VehicleAgency vehicleAgency);
        
    }
}
