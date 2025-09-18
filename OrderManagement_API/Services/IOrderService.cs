using OrderManagement_API.DTOs;

namespace OrderManagement_API.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken ct = default);
        Task<OrderReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<OrderReadDto> CreateAsync(OrderCreateDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, OrderUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        // New
        Task<IEnumerable<OrderReadDto>> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<OrderItemReadDto>> GetItemsByOrderIdAsync(int orderId, CancellationToken ct = default);
        Task<OrderItemReadDto?> AddItemAsync(int orderId, OrderItemCreateDto dto, CancellationToken ct = default);
    }
}