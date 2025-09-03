using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Scripting;
using User_API.DTOs;
using User_API.Models;
using User_API.Repositories;
using UserManagement_API.DTOs;

namespace Users_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        // Map thủ công Entity -> DTO đọc
        private static UserReadDto MapToReadDto(User u) => new()
        {
            UserID = u.UserID,
            UsersName = u.UsersName,
            IsHotelOwner = u.IsHotelOwner,
            IsTourAgency = u.IsTourAgency,
            IsVehicleAgency = u.IsVehicleAgency,
            IsWebAdmin = u.IsWebAdmin,
            IsSupervisor = u.IsSupervisor,
            IsActive = u.IsActive,
            CountWarning = u.CountWarning,
            CreatedAt = u.CreatedAt,
            otp_code = u.otp_code,
            otp_expires = u.otp_expires,
            is_verified = u.is_verified
        };

        public async Task<IEnumerable<UserReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(MapToReadDto);
        }

        public async Task<UserReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            return u == null ? null : MapToReadDto(u);
        }

        public async Task<UserReadDto?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var u = await _repo.GetByUsernameAsync(username, ct);
            return u == null ? null : MapToReadDto(u);
        }

        public async Task<UserReadDto> CreateAsync(UserCreateDto dto, CancellationToken ct = default)
        {
            var existed = await _repo.ExistsByUserNameAsync(dto.UsersName, ct);
            if (existed) throw new InvalidOperationException("UsersName already exists");

            var entity = new User
            {
                UsersName = dto.UsersName,
                // CẦN package BCrypt.Net-Next để dùng dòng dưới:
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                IsHotelOwner = dto.IsHotelOwner,
                IsTourAgency = dto.IsTourAgency,
                IsVehicleAgency = dto.IsVehicleAgency,
                IsWebAdmin = dto.IsWebAdmin,
                IsSupervisor = dto.IsSupervisor,

                otp_code = dto.otp_code,
                otp_expires = dto.otp_expires,

                IsActive = true,
                is_verified = false,
                CountWarning = 0,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _repo.CreateAsync(entity, ct);
            return MapToReadDto(saved);
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto, CancellationToken ct = default)
        {
            if (id != dto.UserID) return false;

            var u = await _repo.GetByIdAsync(id, ct);
            if (u == null) return false;

            u.UsersName = dto.UsersName;
            u.IsHotelOwner = dto.IsHotelOwner;
            u.IsTourAgency = dto.IsTourAgency;
            u.IsVehicleAgency = dto.IsVehicleAgency;
            u.IsWebAdmin = dto.IsWebAdmin;
            u.IsSupervisor = dto.IsSupervisor;

            if (dto.IsActive.HasValue) u.IsActive = dto.IsActive.Value;
            if (dto.CountWarning.HasValue) u.CountWarning = dto.CountWarning.Value;
            if (dto.otp_code is not null) u.otp_code = dto.otp_code;
            if (dto.otp_expires.HasValue) u.otp_expires = dto.otp_expires.Value;
            if (dto.is_verified.HasValue) u.is_verified = dto.is_verified.Value;

            if (u.CountWarning >= 5) u.IsActive = false;

            await _repo.UpdateAsync(u, ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            if (u == null) return false;

            await _repo.DeleteAsync(u, ct);
            return true;
        }

        public async Task<UserReadDto?> GenerateOtpAsync(int userId, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(userId, ct);
            if (u == null) return null;

            var rnd = new Random();
            u.otp_code = rnd.Next(100000, 999999).ToString();
            u.otp_expires = DateTime.UtcNow.AddMinutes(5);
            u.is_verified = false;

            var saved = await _repo.UpdateAsync(u, ct);
            return MapToReadDto(saved);
        }

        public async Task<bool> VerifyOtpAsync(int userId, string otp_code, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(userId, ct);
            if (u == null) return false;

            if (u.otp_code == otp_code && u.otp_expires.HasValue && u.otp_expires > DateTime.UtcNow)
            {
                u.is_verified = true;
                u.otp_code = null;
                u.otp_expires = null;
                await _repo.UpdateAsync(u, ct);
                return true;
            }

            return false;
        }
    }
}
