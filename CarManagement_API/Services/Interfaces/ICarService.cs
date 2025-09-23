using CarManagement_API.DTOs;

namespace CarManagement_API.Services.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarReadDto>> GetAllAsync();
        Task<CarReadDto?> GetByIdAsync(Guid id);
        Task<CarReadDto> CreateAsync(CarCreateDto dto);
        Task<bool> UpdateAsync(Guid id, CarUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CarReadDto>> GetFilteredCarsAsync(
            string? transmission = null,
            string? fuel = null,
            string? carBrand = null,
            string? carName = null,
            int? engineCc = null
        );
    }
}
