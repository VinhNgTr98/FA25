using UserCouponManagement_API.DTOs;

namespace UserCouponManagement_API.Services.Interfaces
{
    public interface IUserCouponService
    {
        Task<IEnumerable<UserCouponReadDTO>> GetAllAsync();
        Task<UserCouponReadDTO?> GetByIdAsync(int id);
        Task<UserCouponReadDTO> CreateAsync(UserCouponCreateDTO dto);
        Task<bool> UpdateAsync(int id, UserCouponUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
