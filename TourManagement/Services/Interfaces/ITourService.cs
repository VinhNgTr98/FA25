using TourManagement.DTOs;

namespace TourManagement.Services.Interfaces
{
    public interface ITourService
    {

        IQueryable<TourReadDTO> GetAllToursAsync();
        Task<TourReadDTO?> GetTourByIdAsync(Guid id);
        Task<TourReadDTO> CreateTourAsync(TourCreateDTO bookCreateDto);
        Task<bool> UpdateTourAsync(TourUpdateDTO bookUpdateDto);
        Task<bool> DeleteTourAsync(Guid id);
    }
}
