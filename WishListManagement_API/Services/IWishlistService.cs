using WishListManagement_API.DTOs;

namespace WishListManagement_API.Services
{
    public interface IWishlistService
    {
        Task<WishlistDto> CreateAsync(CreateWishlistDto dto, CancellationToken ct = default);
        Task<WishlistDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<WishlistDto>> GetByUserAsync(int userId, CancellationToken ct = default);
        Task UpdateAsync(int id, UpdateWishlistDto dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<WishlistDto>> GetAllAsync(WishlistTargetType? targetType = null, CancellationToken ct = default);
    }
}
