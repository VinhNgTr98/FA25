using TourManagement.DTOs;

namespace TourManagement.Services.Interfaces
{
    public interface IItineraryService
    {
        Task<IEnumerable<ItineraryReadDTO>> GetAllAsync();
        Task<ItineraryReadDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<ItineraryReadDTO>> GetByTourIdAsync(Guid tourId);
        Task<ItineraryReadDTO> CreateAsync(ItineraryCreateDTO dto);
        Task<bool> UpdateAsync(Guid id, ItineraryUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
