using AutoMapper;
using CouponManagement_API.DTOs;
using CouponManagement_API.Models;
using CouponManagement_API.Repositories.Interfaces;
using CouponManagement_API.Services.Interfaces;

namespace CouponManagement_API.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _repo;
        private readonly IMapper _mapper;

        public CouponService(ICouponRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CouponReadDTO>> GetAllCouponsAsync()
        {
            var coupons = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CouponReadDTO>>(coupons);
        }

        public async Task<CouponReadDTO?> GetCouponByIdAsync(int id)
        {
            var coupon = await _repo.GetByIdAsync(id);
            return coupon == null ? null : _mapper.Map<CouponReadDTO>(coupon);
        }

        public async Task<CouponReadDTO> CreateCouponAsync(CouponCreateDTO dto)
        {
            var coupon = _mapper.Map<Coupon>(dto);
            await _repo.AddAsync(coupon);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CouponReadDTO>(coupon);
        }

        public async Task<bool> UpdateCouponAsync(int id, CouponUpdateDTO dto)
        {
            var existingCoupon = await _repo.GetByIdAsync(id);
            if (existingCoupon == null) return false;

            _mapper.Map(dto, existingCoupon);
            await _repo.UpdateAsync(existingCoupon);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCouponAsync(int id)
        {
            var existingCoupon = await _repo.GetByIdAsync(id);
            if (existingCoupon == null) return false;

            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
