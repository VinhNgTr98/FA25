using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookingManagement_API.Data;
using BookingManagement_API.Models;

namespace BookingManagement_API.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingManagement_APIContext _db;
        public BookingRepository(BookingManagement_APIContext db) => _db = db;

        public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken ct = default)
            => await _db.Booking
                        .AsNoTracking()
                        .Include(b => b.Items)
                        .ToListAsync(ct);

        public Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.Booking
                  .AsNoTracking()
                  .Include(b => b.Items)
                  .FirstOrDefaultAsync(b => b.BookingId == id, ct);

        public async Task<Booking> AddAsync(Booking booking, CancellationToken ct = default)
        {
            _db.Booking.Add(booking);
            await _db.SaveChangesAsync(ct);
            return booking;
        }

        public async Task<bool> UpdateAsync(Booking booking, CancellationToken ct = default)
        {
            _db.Booking.Update(booking);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _db.Booking.FindAsync(new object?[] { id }, ct);
            if (existing == null) return false;
            _db.Booking.Remove(existing);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId, CancellationToken ct = default)
            => await _db.Booking
                        .AsNoTracking()
                        .Include(b => b.Items)
                        .Where(b => b.UserID == userId)
                        .ToListAsync(ct);

        public async Task<IEnumerable<BookingItem>> GetItemsByOrderIdAsync(int bookingId, CancellationToken ct = default)
            => await _db.BookingItems
                        .AsNoTracking()
                        .Where(i => i.BookingId == bookingId)
                        .ToListAsync(ct);

        public async Task<BookingItem> AddItemAsync(BookingItem item, CancellationToken ct = default)
        {
            _db.BookingItems.Add(item);
            await _db.SaveChangesAsync(ct);
            return item;
        }
        public async Task<int> DeletenotefisnitBoking(TimeSpan ttl, CancellationToken ct = default)
        {
            var threshold = DateTime.UtcNow - ttl;

            // Lấy danh sách id booking quá hạn và đang Pending
            var expiredBookingIds = await _db.Booking
                .Where(b => b.Status == "Pending" && b.CreatedAt < threshold)
                .Select(b => b.BookingId)
                .ToListAsync(ct);

            if (expiredBookingIds.Count == 0) return 0;

            // Xóa BookingItem trước (nếu chưa cấu hình cascade)
            var items = await _db.BookingItems
                .Where(i => expiredBookingIds.Contains(i.BookingId))
                .ToListAsync(ct);

            if (items.Count > 0)
                _db.BookingItems.RemoveRange(items);

            // Xóa Booking
            var bookings = await _db.Booking
                .Where(b => expiredBookingIds.Contains(b.BookingId))
                .ToListAsync(ct);

            _db.Booking.RemoveRange(bookings);

            await _db.SaveChangesAsync(ct);
            return bookings.Count;
        }
    }
}