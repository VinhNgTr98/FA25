

using MotorbikeManageMent_API.DTOs;

namespace MotorbikeManageMent_API.Services.Interfaces
{
    public interface IMotorbikeService
    {
        Task<IEnumerable<MotorbikeReadDto>> GetAllAsync();
        Task<MotorbikeReadDto?> GetByIdAsync(Guid id);
        Task<MotorbikeReadDto> CreateAsync(MotorbikeCreateDto dto);
        Task<bool> UpdateAsync(Guid id, MotorbikeUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);

        // Phương thức lọc theo Transmission, Fuel, MotorbikeBrand, MotorbikeName, EngineCc
        Task<IEnumerable<MotorbikeReadDto>> GetFilteredMotorbikesAsync(
            string? transmission = null,
            string? fuel = null,
            string? motorbikeBrand = null,
            string? motorbikeName = null,
            int? engineCc = null
        );
    }
}
