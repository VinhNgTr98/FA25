using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagement_API.Data;      // <-- namespace của OrderManagement_APIContext
using OrderManagement_API.Models;    // <-- namespace chứa model Order

namespace OrderManagement_API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderManagement_APIContext _db;

        public OrderRepository(OrderManagement_APIContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            // EF Core async + không tracking để đọc nhanh
            return await _db.Order.AsNoTracking().ToListAsync();
        }

        public Task<Order?> GetByIdAsync(int id)
        {
            return _db.Order.AsNoTracking()
                     .FirstOrDefaultAsync(o => o.OrderID == id);
        }

        public async Task<Order> AddAsync(Order entity)
        {
            _db.Order.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Order?> UpdateAsync(Order entity)
        {
            // Lấy entity đang có trong DB
            var current = await _db.Order.FirstOrDefaultAsync(o => o.OrderID == entity.OrderID);
            if (current == null) return null;

            // Gán giá trị mới (không đụng PK)
            _db.Entry(current).CurrentValues.SetValues(entity);

            await _db.SaveChangesAsync();
            return current;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var current = await _db.Order.FirstOrDefaultAsync(o => o.OrderID == id);
            if (current == null) return false;

            _db.Order.Remove(current);
            return (await _db.SaveChangesAsync()) > 0;
        }
    }
}
