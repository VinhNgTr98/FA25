using CartManagement_Api.Data;
using CartManagement_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CartManagement_Api.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartManagement_ApiContext _db;
        public CartRepository(CartManagement_ApiContext db) => _db = db;

        public Task<Cart?> GetCartWithItemsByUserAsync(int userId, CancellationToken ct) =>
            _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserID == userId, ct);

        public Task<Cart?> GetCartWithItemsByIdAsync(int cartId, CancellationToken ct) =>
            _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.CartID == cartId, ct);

        public Task<CartItem?> GetCartItemByIdAsync(int cartItemId, CancellationToken ct) =>
            _db.CartItems.Include(i => i.Cart).FirstOrDefaultAsync(i => i.CartItemID == cartItemId, ct);

        public async Task AddCartAsync(Cart cart, CancellationToken ct)
        {
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}