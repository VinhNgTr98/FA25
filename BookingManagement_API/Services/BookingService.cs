using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingManagement_API.DTOs;
using BookingManagement_API.Models;
using BookingManagement_API.Repositories;

namespace BookingManagement_API.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;

        public BookingService(IBookingRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<BookingReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            var bookings = await _repo.GetAllAsync(ct);
            return bookings.Select(MapToReadDto);
        }

        public async Task<BookingReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var booking = await _repo.GetByIdAsync(id, ct);
            return booking == null ? null : MapToReadDto(booking);
        }

        public async Task<BookingReadDto> CreateAsync(BookingCreateDto dto, CancellationToken ct = default)
        {
            var booking = new Booking
            {
                UserID = dto.UserID,
                BookingDate = DateTime.UtcNow,
                Status = "Pending",
                TaxAmount = dto.TaxAmount,
                CouponId = dto.CouponId,
                BookingNote = dto.BookingNote,
                TotalAmount = dto.TaxAmount ?? 0m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _repo.AddAsync(booking, ct);
            return MapToReadDto(created);
        }

        public async Task<bool> UpdateAsync(int id, BookingUpdateDto dto, CancellationToken ct = default)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return false;

            existing.TotalAmount = dto.TotalAmount;
            existing.Status = dto.Status;
            existing.TaxAmount = dto.TaxAmount;
            existing.CouponId = dto.CouponId;
            existing.BookingNote = dto.BookingNote;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(existing, ct);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
            => _repo.DeleteAsync(id, ct);

        public async Task<IEnumerable<BookingReadDto>> GetByUserIdAsync(int userId, CancellationToken ct = default)
        {
            var bookings = await _repo.GetByUserIdAsync(userId, ct);
            return bookings.Select(MapToReadDto);
        }

        public async Task<IEnumerable<BookingItemReadDto>> GetItemsByOrderIdAsync(int BookingId, CancellationToken ct = default)
        {
            var items = await _repo.GetItemsByOrderIdAsync(BookingId, ct);
            return items.Select(MapToItemReadDto);
        }

        public async Task<BookingItemReadDto?> AddItemAsync(int BookingId, BookingItemCreateDto dto, CancellationToken ct = default)
        {
            var booking = await _repo.GetByIdAsync(BookingId, ct);
            if (booking == null) return null;

            var item = new BookingItem
            {
                BookingId = BookingId,
                ItemType = dto.ItemType,
                ItemID = dto.ItemID,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            var created = await _repo.AddItemAsync(item, ct);

            // Recalculate total = sum(items) + tax
            var allItems = await _repo.GetItemsByOrderIdAsync(BookingId, ct);
            var itemsTotal = allItems.Sum(i => i.Quantity * i.UnitPrice);
            var tax = booking.TaxAmount ?? 0m;
            booking.TotalAmount = Math.Round(itemsTotal + tax, 2, MidpointRounding.AwayFromZero);
            booking.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(booking, ct);

            return MapToItemReadDto(created);
        }

        public async Task<int> CleanupExpiredPendingAsync(TimeSpan ttl, CancellationToken ct = default)
        {
            // TTL mặc định bạn có thể dùng TimeSpan.FromMinutes(5)
            return await _repo.DeletenotefisnitBoking(ttl, ct);
        }

        private static BookingReadDto MapToReadDto(Booking b)
        {
            return new BookingReadDto
            {
                BookingId = b.BookingId,
                UserID = b.UserID,
                BookingDate = b.BookingDate,
                TotalAmount = b.TotalAmount,
                Status = b.Status,
                TaxAmount = b.TaxAmount,
                CouponId = b.CouponId,
                BookingNote = b.BookingNote,
                Items = (b.Items ?? new List<BookingItem>())
                        .Select(MapToItemReadDto)
                        .ToList()
            };
        }

        private static BookingItemReadDto MapToItemReadDto(BookingItem i)
        {
            return new BookingItemReadDto
            {
                BookingItemID = i.BookingItemID,
                ItemType = i.ItemType,
                ItemID = i.ItemID,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                StartDate = i.StartDate,
                EndDate = i.EndDate
            };
        }
    }
}