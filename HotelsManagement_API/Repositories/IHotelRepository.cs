using HotelsManagement_API.Models;

namespace HotelsManagement_API.Repositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel?> GetByIdAsync(Guid id);
        Task<Hotel> AddAsync(Hotel hotel);
        Task<Hotel?> UpdateAsync(Hotel hotel);
        Task<bool> DeleteAsync(Guid id);
    }
}
