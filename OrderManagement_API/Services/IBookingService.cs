using OrderManagement_API.DTOs;

namespace OrderManagement_API.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingReadDto>> GetAllAsync(CancellationToken ct = default);
        Task<BookingReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<BookingReadDto> CreateAsync(BookingCreateDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, BookingUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        // New
        Task<IEnumerable<BookingReadDto>> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<BookingItemReadDto>> GetItemsByOrderIdAsync(int orderId, CancellationToken ct = default);
        Task<BookingItemReadDto?> AddItemAsync(int orderId, BookingItemCreateDto dto, CancellationToken ct = default);
    }
}