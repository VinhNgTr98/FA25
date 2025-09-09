using System.Threading;
using System.Threading.Tasks;
using CartManagement_Api.DTOs;

namespace CartManagement_Api.Services
{
    public interface ICartService
    {
        Task<CartReadDto> GetCartByUserIdAsync(int userId, CancellationToken ct = default);
        Task<CartItemReadDto> AddItemAsync(int userId, CartItemCreateDto dto, CancellationToken ct = default);
        Task<CartItemReadDto?> UpdateItemAsync(int cartItemId, CartItemUpdateDto dto, CancellationToken ct = default);
        Task<bool> RemoveItemAsync(int cartItemId, CancellationToken ct = default);
        Task<bool> ClearCartAsync(int userId, CancellationToken ct = default);
    }
}