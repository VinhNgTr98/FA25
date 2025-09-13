using CartManagement_Api.DTOs;

namespace CartManagement_Api.Services
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItemReadDto>> GetAllAsync();
        Task<CartItemReadDto?> GetByIdAsync(int cartItemId);
        Task<CartItemReadDto> CreateAsync(CartItemCreateDto dto);
        Task<CartItemReadDto?> UpdateAsync(int cartItemId, CartItemUpdateDto dto);
        Task<bool> DeleteAsync(int cartItemId);
    }
}