using TourManagement.Model;

namespace TourManagement.Repositories.Interfaces
{
    public interface ITourMemberRepository
    {
        Task<IEnumerable<TourMember>> GetAllAsync();
        Task<TourMember?> GetByIdAsync(Guid id);
        Task<TourMember> AddAsync(TourMember member);
        Task<bool> UpdateAsync(TourMember member);
        Task<bool> DeleteAsync(Guid id);
    }
}
