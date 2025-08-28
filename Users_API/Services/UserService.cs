using AutoMapper;
using Microsoft.AspNetCore.Identity;
using User_API.DTOs;
using User_API.Models;
using User_API.Repositories;
using UserManagement_API.DTOs;

namespace Users_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserReadDto?> GetAsync(int id, CancellationToken ct)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            return u == null ? null : _mapper.Map<UserReadDto>(u);
        }

        public async Task<List<UserReadDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _repo.GetAllAsync(ct);
            return _mapper.Map<List<UserReadDto>>(list);
        }

        public async Task<UserReadDto> CreateAsync(UserCreateDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<User>(dto);

            // Hash mật khẩu (dùng thư viện BCrypt.Net-Next hoặc tự hash)
            // entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            // tạm thời nếu đã nhận PasswordHash trong dto thì:
            entity.PasswordHash = dto.Password; // đổi theo cách bạn chọn

            var saved = await _repo.AddAsync(entity, ct);
            return _mapper.Map<UserReadDto>(saved);
        }

        public async Task<UserReadDto?> UpdateAsync(UserUpdateDto dto, CancellationToken ct)
        {
            var exist = await _repo.GetByIdAsync(dto.UserID, ct);
            if (exist == null) return null;

            // Map từ dto -> entity (chỉ các field cho phép)
            exist.UsersName = dto.UsersName;
            exist.IsHotelOwner = dto.IsHotelOwner;
            exist.IsTourAgency = dto.IsTourAgency;
            exist.IsVehicleAgency = dto.IsVehicleAgency;
            exist.IsWebAdmin = dto.IsWebAdmin;
            exist.IsSupervisor = dto.IsSupervisor;
            if (dto.IsActive.HasValue) exist.IsActive = dto.IsActive.Value;

            var updated = await _repo.UpdateAsync(exist, ct);
            return updated == null ? null : _mapper.Map<UserReadDto>(updated);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct) =>
            _repo.DeleteAsync(id, ct);

        public async Task<UserReadDto?> IncreaseWarningAsync(int userId, int increaseBy, CancellationToken ct)
        {
            var u = await _repo.GetByIdAsync(userId, ct);
            if (u == null) return null;

            u.CountWarning += Math.Max(1, increaseBy);
            if (u.CountWarning >= 5) u.IsActive = false;

            var saved = await _repo.UpdateAsync(u, ct);
            return saved == null ? null : _mapper.Map<UserReadDto>(saved);
        }

        public async Task<UserReadDto?> GetByUsernameAsync(string username, CancellationToken ct)
        {
            var u = await _repo.GetByUsernameAsync(username, ct);
            return u == null ? null : _mapper.Map<UserReadDto>(u);
        }
    }
}
