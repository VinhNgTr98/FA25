using CartManagement_Api.Data;
using CartManagement_Api.Models;
using System.Data.Entity;

namespace CartManagement_Api.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartManagement_ApiContext _db;
        public CartRepository(CartManagement_ApiContext db) => _db = db;

        public Task<Cart?> GetByUserIdAsync(int userId, CancellationToken ct = default)
            => _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserID == userId, ct);

        public async Task<Cart> CreateAsync(int userId, CancellationToken ct = default)
        {
            var cart = new Cart { UserID = userId };
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync(ct);
            return cart;
        }

        public Task<CartItem?> FindItemAsync(int cartId, CartItemType type, Guid itemId, DateTime? start, DateTime? end, CancellationToken ct = default)
            => _db.CartItems.FirstOrDefaultAsync(i =>
                    i.CartID == cartId &&
                    i.ItemType == type &&
                    i.ItemID == itemId &&
                    i.StartDate == start &&
                    i.EndDate == end, ct);

        public Task<CartItem?> GetItemByIdAsync(int cartItemId, CancellationToken ct = default)
            => _db.CartItems.FirstOrDefaultAsync(i => i.CartItemID == cartItemId, ct);

        public async Task AddItemAsync(CartItem item, CancellationToken ct = default)
        {
            _db.CartItems.Add(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> RemoveItemAsync(int cartId, int cartItemId, CancellationToken ct = default)
        {
            var it = await _db.CartItems.FirstOrDefaultAsync(i => i.CartItemID == cartItemId && i.CartID == cartId, ct);
            if (it == null) return false;
            _db.CartItems.Remove(it);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> ClearAsync(int cartId, CancellationToken ct = default)
        {
            var items = _db.CartItems.Where(i => i.CartID == cartId);
            _db.CartItems.RemoveRange(items);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
    }
}
