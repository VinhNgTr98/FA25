using TourManagement.Model;

namespace TourManagement.Repositories.Interfaces
{
    public interface ITourGuideRepository
    {
        Task<IEnumerable<TourGuide>> GetAllAsync();
        Task<TourGuide?> GetByIdAsync(Guid id);
        Task<TourGuide> AddAsync(TourGuide guide);
        Task<bool> UpdateAsync(TourGuide guide);
        Task<bool> DeleteAsync(Guid id);
    }
}
