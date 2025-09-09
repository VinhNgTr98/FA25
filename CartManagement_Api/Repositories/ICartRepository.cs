using CartManagement_Api.Models;

namespace CartManagement_Api.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<Cart> CreateAsync(int userId, CancellationToken ct = default);
        Task<CartItem?> FindItemAsync(int cartId, CartItemType type, Guid itemId, DateTime? start, DateTime? end, CancellationToken ct = default);
        Task<CartItem?> GetItemByIdAsync(int cartItemId, CancellationToken ct = default);
        Task AddItemAsync(CartItem item, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task<bool> RemoveItemAsync(int cartId, int cartItemId, CancellationToken ct = default);
        Task<bool> ClearAsync(int cartId, CancellationToken ct = default);
    }
}
