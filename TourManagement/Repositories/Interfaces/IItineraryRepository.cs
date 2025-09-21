using TourManagement.Model;

namespace TourManagement.Repositories.Interfaces
{
    public interface IItineraryRepository
    {
        Task<IEnumerable<Itinerary>> GetAllAsync();
        Task<Itinerary?> GetByIdAsync(Guid id);
        Task<IEnumerable<Itinerary>> GetByTourIdAsync(Guid tourId);
        Task<Itinerary> CreateAsync(Itinerary itinerary);
        Task<bool> UpdateAsync(Itinerary itinerary);
        Task<bool> DeleteAsync(Guid id);
    }
}
