using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Models;

namespace TourAgencyManagement_API.Services.Interfaces
{
    public interface ITourAgencyService
    {
        Task<IEnumerable<TourAgencyReadDTO>> GetAllAsync();
        Task<TourAgencyReadDTO?> GetByIdAsync(Guid id);
        Task<TourAgencyReadDTO> CreateAsync(TourAgencyCreateDTO dto);
        Task<bool> UpdateAsync(Guid id, TourAgencyUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
