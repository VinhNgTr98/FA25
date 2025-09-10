using CartManagement_Api.DTOs;

namespace CartManagement_Api.Services
{
    public interface ICartService
    {
        Task<CartReadDto> GetOrCreateCartAsync(int userId, CancellationToken ct = default);
        Task<CartReadDto?> GetCartByUserAsync(int userId, CancellationToken ct = default);
        Task<CartReadDto> AddItemAsync(int userId, CartItemCreateDto dto, CancellationToken ct = default);
        Task<CartReadDto> UpdateItemAsync(int userId, int cartItemId, CartItemUpdateDto dto, CancellationToken ct = default);
        Task<bool> RemoveItemAsync(int userId, int cartItemId, string rowVersion, CancellationToken ct = default);
        Task<bool> ClearCartAsync(int userId, CancellationToken ct = default);
        Task<CartSummaryDto> GetSummaryAsync(int userId, CancellationToken ct = default);
        Task<CartReadDto?> GetCartByIdAsync(int cartId, CancellationToken ct = default);
        Task<bool> DeleteCartAsync(int cartId, CancellationToken ct = default);
    }
}