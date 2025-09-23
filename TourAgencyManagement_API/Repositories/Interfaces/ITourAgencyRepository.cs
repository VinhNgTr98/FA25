using TourAgencyManagement_API.Models;

namespace TourAgencyManagement_API.Repositories.Interfaces
{
    public interface ITourAgencyRepository
    {
        Task<IEnumerable<TourAgency>> GetAllAsync();
        Task<TourAgency?> GetByIdAsync(Guid id);
        Task<TourAgency> AddAsync(TourAgency agency);

        Task UpdateAsync(TourAgency agency);
        Task DeleteAsync(Guid id);
        Task<TourAgency?> GetByUserIdAsync(int userId);

    }
}
