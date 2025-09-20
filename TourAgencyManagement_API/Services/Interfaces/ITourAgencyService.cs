using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Models;

namespace TourAgencyManagement_API.Services.Interfaces
{
    public interface ITourAgencyService
    {
        Task<IEnumerable<TourAgencyReadDto>> GetAllAsync();
        Task<TourAgencyReadDto?> GetByIdAsync(Guid id);
        Task<TourAgencyReadDto> CreateAsync(TourAgencyCreateDto dto);
        Task<bool> UpdateAsync(Guid id, TourAgencyUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);

    }
}
