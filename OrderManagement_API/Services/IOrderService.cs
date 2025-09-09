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
    }
}
