using BookingManagement_API.Models;

namespace BookingManagement_API.Repositories
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

        // NEW: xóa các booking Pending quá TTL, trả về số booking bị xóa
        Task<int> DeletenotefisnitBoking(TimeSpan ttl, CancellationToken ct = default);
        
        Task<IEnumerable<BookingItem>> GetItemsByItemTypeAsync(string itemType, CancellationToken ct = default);
        Task<IEnumerable<BookingItem>> GetItemsByBookingAndItemTypeAsync(int bookingId, string itemType, CancellationToken ct = default);
    }
}