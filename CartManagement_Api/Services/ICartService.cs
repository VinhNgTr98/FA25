using CartManagement_Api.DTOs;

namespace CartManagement_Api.Services
{
    public interface ICartService
    {
        Task<CartReadDto> GetOrCreateAsync(int userId, CancellationToken ct = default);
        Task<CartItemReadDto> AddOrIncreaseAsync(int userId, CartItemCreateDto dto, CancellationToken ct = default);
        Task<bool> UpdateItemAsync(int userId, int cartItemId, CartItemUpdateDto dto, CancellationToken ct = default);
        Task<bool> RemoveItemAsync(int userId, int cartItemId, CancellationToken ct = default);
        Task<bool> ClearAsync(int userId, CancellationToken ct = default);
    }
}
