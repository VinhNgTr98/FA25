using CartManagement_Api.Models;

namespace CartManagement_Api.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart?> GetByIdAsync(int cartId);
        Task<Cart> AddAsync(Cart cart);
        Task<Cart> UpdateAsync(Cart cart);
        Task<bool> DeleteAsync(int cartId);
    }
}