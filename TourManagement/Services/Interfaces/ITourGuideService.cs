using TourManagement.DTOs;

namespace TourManagement.Services.Interfaces
{
    public interface ITourGuideService
    {
        Task<IEnumerable<TourGuideReadDTO>> GetAllAsync();
        Task<TourGuideReadDTO?> GetByIdAsync(Guid id);
        Task<TourGuideReadDTO> CreateAsync(TourGuideCreateDTO dto);
        Task<bool> UpdateAsync(TourGuideUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
