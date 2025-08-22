using Microsoft.EntityFrameworkCore;
using System;
using WishListManagement_API.Data;
using WishListManagement_API.DTOs;
using WishListManagement_API.Models;

namespace WishListManagement_API.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly WishListManagement_APIContext _db;
        public WishlistRepository(WishListManagement_APIContext db) => _db = db;

        public async Task<Wishlist> AddAsync(Wishlist item, CancellationToken ct = default)
        {
            _db.Wishlist.Add(item);
            await _db.SaveChangesAsync(ct);
            return item;
        }

        public Task<Wishlist?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _db.Wishlist.FirstOrDefaultAsync(x => x.WishlistId == id, ct);

        public async Task<IReadOnlyList<Wishlist>> GetByUserAsync(int userId, CancellationToken ct = default) =>
            await _db.Wishlist.AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedAt)
                .ToListAsync(ct);

        public async Task<bool> ExistsAsync(int userId, WishlistTargetType type, Guid targetId, CancellationToken ct = default)
        {
            return type switch
            {
                WishlistTargetType.Hotel => await _db.Wishlist.AsNoTracking()
                    .AnyAsync(x => x.UserId == userId && x.HotelId == targetId, ct),

                WishlistTargetType.Vehicle => await _db.Wishlist.AsNoTracking()
                    .AnyAsync(x => x.UserId == userId && x.VehiclesId == targetId, ct),

                WishlistTargetType.Tour => await _db.Wishlist.AsNoTracking()
                    .AnyAsync(x => x.UserId == userId && x.TourId == targetId, ct),

                WishlistTargetType.Service => await _db.Wishlist.AsNoTracking()
                    .AnyAsync(x => x.UserId == userId && x.ServiceId == targetId, ct),

                _ => false
            };
        }

        public async Task UpdateAsync(Wishlist item, CancellationToken ct = default)
        {
            _db.Wishlist.Update(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _db.Wishlist.FindAsync(new object?[] { id }, ct);
            if (entity is null) return;
            _db.Wishlist.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
