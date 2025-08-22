using OrderManagement_API.DTOs;

namespace OrderManagement_API.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>> GetAllAsync();
        Task<OrderReadDto?> GetByIdAsync(int id);
        Task<OrderReadDto> AddAsync(OrderCreateDto dto);
        Task<OrderReadDto?> UpdateAsync(int id, OrderUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
