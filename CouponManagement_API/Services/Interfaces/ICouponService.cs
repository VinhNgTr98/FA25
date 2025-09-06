using CouponManagement_API.DTOs;

namespace CouponManagement_API.Services.Interfaces
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponReadDTO>> GetAllCouponsAsync();
        Task<CouponReadDTO?> GetCouponByIdAsync(int id);
        Task<CouponReadDTO> CreateCouponAsync(CouponCreateDTO dto);
        Task<bool> UpdateCouponAsync(int id, CouponUpdateDTO dto);
        Task<bool> DeleteCouponAsync(int id);
    }
}
