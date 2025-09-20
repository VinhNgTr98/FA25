using OrderManagement_API.Models;

namespace OrderManagement_API.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync(CancellationToken ct = default);
        Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Booking> AddAsync(Booking booking, CancellationToken ct = default);
        Task<bool> UpdateAsync(Booking booking, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        // New
        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<BookingItem>> GetItemsByOrderIdAsync(int bookingId, CancellationToken ct = default);
        Task<BookingItem> AddItemAsync(BookingItem item, CancellationToken ct = default);
    }
}