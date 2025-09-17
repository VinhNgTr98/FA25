using Microsoft.EntityFrameworkCore;
using OrderManagement_API.Data;
using OrderManagement_API.Models;

namespace OrderManagement_API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderManagement_APIContext _db;
        public OrderRepository(OrderManagement_APIContext db) => _db = db;

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default)
    => await _db.Orders.AsNoTracking().Include(o => o.Items).ToListAsync(ct);

        public Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
    => _db.Orders.AsNoTracking().Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderID == id, ct);

        public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);
            return order;
        }

        public async Task<bool> UpdateAsync(Order order, CancellationToken ct = default)
        {
            _db.Orders.Update(order);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var order = await _db.Orders.FindAsync(new object?[] { id }, ct);
            if (order == null) return false;
            _db.Orders.Remove(order);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId, CancellationToken ct = default)
    => await _db.Orders.AsNoTracking().Include(o => o.Items).Where(o => o.UserID == userId).ToListAsync(ct);

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId, CancellationToken ct = default)
            => await _db.OrderItems
                        .Where(i => i.OrderID == orderId)
                        .ToListAsync(ct);

        public async Task<OrderItem> AddItemAsync(OrderItem item, CancellationToken ct = default)
        {
            _db.OrderItems.Add(item);
            await _db.SaveChangesAsync(ct);
            return item;
        }
    }
}