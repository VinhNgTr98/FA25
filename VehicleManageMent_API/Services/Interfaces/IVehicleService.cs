using VehicleManageMent_API.DTOs;

namespace VehicleManageMent_API.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleReadDTO>> GetAllAsync();
        Task<VehicleReadDTO?> GetByIdAsync(Guid id);
        Task<VehicleReadDTO> CreateAsync(VehicleCreateDTO dto);
        Task<bool> UpdateAsync(Guid id, VehicleUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
