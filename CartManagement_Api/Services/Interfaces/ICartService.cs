using CartManagement_Api.DTOs;

namespace CartManagement_Api.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartReadDto>> GetAllAsync();
        Task<CartReadDto?> GetByIdAsync(int cartId);
        Task<CartReadDto> CreateAsync(CartCreateDto dto);
        Task<CartReadDto?> UpdateAsync(int cartId, CartUpdateDto dto);
        Task<bool> DeleteAsync(int cartId);
    }
}