using VehicleAgencyManagement_API.DTOs;

namespace VehicleAgencyManagement_API.Services.Interfaces
{
    public interface IVehicleAgencyService
    {
        Task<IEnumerable<VehicleAgencyReadDTO>> GetAllAsync();
        Task<VehicleAgencyReadDTO?> GetByIdAsync(Guid id);
        Task<VehicleAgencyReadDTO> CreateAsync(VehicleAgencyCreateDTO dto);
        Task<bool> UpdateAsync(Guid id, VehicleAgencyUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
