using Microsoft.EntityFrameworkCore;
using OrderManagement_API.Data;
using OrderManagement_API.Models;

namespace OrderManagement_API.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingManagement_APIContext _db;
        public BookingRepository(BookingManagement_APIContext db) => _db = db;

        public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken ct = default)
    => await _db.Orders.AsNoTracking().Include(o => o.Items).ToListAsync(ct);

        public Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default)
    => _db.Orders.AsNoTracking().Include(o => o.Items).FirstOrDefaultAsync(o => o.BookingId == id, ct);

        public async Task<Booking> AddAsync(Booking order, CancellationToken ct = default)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);
            return order;
        }

        public async Task<bool> UpdateAsync(Booking order, CancellationToken ct = default)
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

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId, CancellationToken ct = default)
    => await _db.Orders.AsNoTracking().Include(o => o.Items).Where(o => o.UserID == userId).ToListAsync(ct);

        public async Task<IEnumerable<BookingItem>> GetItemsByOrderIdAsync(int orderId, CancellationToken ct = default)
            => await _db.OrderItems
                        .Where(i => i.BookingId == orderId)
                        .ToListAsync(ct);

        public async Task<BookingItem> AddItemAsync(BookingItem item, CancellationToken ct = default)
        {
            _db.OrderItems.Add(item);
            await _db.SaveChangesAsync(ct);
            return item;
        }
    }
}