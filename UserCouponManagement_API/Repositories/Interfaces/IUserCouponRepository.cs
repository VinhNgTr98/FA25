using UserCouponManagement_API.Models;

namespace UserCouponManagement_API.Repositories.Interfaces
{
    public interface IUserCouponRepository
    {
        Task<IEnumerable<UserCoupon>> GetAllAsync();
        Task<UserCoupon?> GetByIdAsync(int id);
        Task AddAsync(UserCoupon entity);
        Task UpdateAsync(UserCoupon entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
