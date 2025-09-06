using AutoMapper;
using UserCouponManagement_API.DTOs;
using UserCouponManagement_API.Models;
using UserCouponManagement_API.Repositories.Interfaces;
using UserCouponManagement_API.Services.Interfaces;

namespace UserCouponManagement_API.Services
{
    public class UserCouponService : IUserCouponService
    {
        private readonly IUserCouponRepository _repo;
        private readonly IMapper _mapper;

        public UserCouponService(IUserCouponRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserCouponReadDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserCouponReadDTO>>(list);
        }

        public async Task<UserCouponReadDTO?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<UserCouponReadDTO>(entity);
        }

        public async Task<UserCouponReadDTO> CreateAsync(UserCouponCreateDTO dto)
        {
            var entity = _mapper.Map<UserCoupon>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<UserCouponReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, UserCouponUpdateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
