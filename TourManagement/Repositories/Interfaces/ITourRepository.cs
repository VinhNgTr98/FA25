using TourManagement.Model;

namespace TourManagement.Repositories.Interfaces
{
    public interface ITourRepository
    {
        IQueryable<Tour> GetAllAsync();
        Task<Tour?> GetByIdAsync(Guid id);
        Task AddAsync(Tour tour);
        void Update(Tour tour);
        void Delete(Tour tour);
        Task<bool> SaveChangesAsync();
    }
}
