using OrderManagement_API.Models;

namespace OrderManagement_API.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default);
        Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Order> AddAsync(Order order, CancellationToken ct = default);
        Task<bool> UpdateAsync(Order order, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        // New
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId, CancellationToken ct = default);
        Task<OrderItem> AddItemAsync(OrderItem item, CancellationToken ct = default);
    }
}