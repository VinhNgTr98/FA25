using TourManagement.DTOs;

namespace TourManagement.Services.Interfaces
{
    public interface ITourMemberService
    {
        Task<IEnumerable<TourMemberReadDTO>> GetAllAsync();
        Task<TourMemberReadDTO?> GetByIdAsync(Guid id);
        Task<TourMemberReadDTO> CreateAsync(TourMemberCreateDTO dto);
        Task<bool> UpdateAsync(TourMemberUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
