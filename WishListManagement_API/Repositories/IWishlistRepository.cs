using WishListManagement_API.DTOs;
using WishListManagement_API.Models;

namespace WishListManagement_API.Repositories
{
    public interface IWishlistRepository
    {
        Task<Wishlist> AddAsync(Wishlist item, CancellationToken ct = default);
        Task<Wishlist?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Wishlist>> GetByUserAsync(int userId, CancellationToken ct = default);
        Task<bool> ExistsAsync(int userId, WishlistTargetType type, Guid targetId, CancellationToken ct = default);
        Task UpdateAsync(Wishlist item, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Wishlist>> GetAllAsync(CancellationToken ct = default);
    }
}
