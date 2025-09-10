using CartManagement_Api.Models;

namespace CartManagement_Api.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartWithItemsByUserAsync(int userId, CancellationToken ct);
        Task<Cart?> GetCartWithItemsByIdAsync(int cartId, CancellationToken ct);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId, CancellationToken ct);
        Task AddCartAsync(Cart cart, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}