using AutoMapper;
using User_API.DTOs;
using User_API.Models;
using User_API.Repositories;
using UserManagement_API.DTOs;
using System.Security.Cryptography;
using UserManagement_API.Services;

namespace UserManagement_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IEmailSender _email;
        private readonly IOtpLimiter _limiter;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IEmailSender email, IOtpLimiter limiter, IMapper mapper)
        {
            _repo = repo;
            _email = email;
            _limiter = limiter;
            _mapper = mapper;
        }

        // ========== READ ==========
        public async Task<IEnumerable<UserReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(u => _mapper.Map<UserReadDto>(u));
        }

        public async Task<UserReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            return u == null ? null : _mapper.Map<UserReadDto>(u);
        }

        public async Task<UserReadDto?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var u = await _repo.GetByEmailAsync(email, ct);
            return u == null ? null : _mapper.Map<UserReadDto>(u);
        }

        // ========== CREATE / REGISTER + OTP ==========
        public async Task<(UserReadDto User, bool IsResend)> CreateWithInfoAsync(
            UserWithInfoCreateDto dto, CancellationToken ct = default)
        {
            // 1) Kiểm tra tồn tại
            var existing = await _repo.GetByEmailAsync(dto.Email, ct);
            if (existing != null)
            {
                if (existing.is_verified)
                    throw new InvalidOperationException("EMAIL_EXISTS"); // 409

                if (!await _limiter.CanSendAsync(existing.UserID, ct))
                    throw new InvalidOperationException("OTP_RATE_LIMIT"); // 429

                existing.otp_code = GenerateOtp6();
                existing.otp_expires = DateTime.UtcNow.AddMinutes(5);
                existing.is_verified = false;

                var saved = await _repo.UpdateAsync(existing, ct);
                await SendOtpEmailAsync(saved, ct);
                await _limiter.RecordSendAsync(saved.UserID, ct);

                return (_mapper.Map<UserReadDto>(saved), true);
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new InvalidOperationException("PASSWORD_REQUIRED");

            // 2) Tạo mới: map DTO -> Entity
            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.is_verified = false;
            user.IsActive = false;
            user.CreatedAt = DateTime.UtcNow;
            user.otp_code = GenerateOtp6();
            user.otp_expires = DateTime.UtcNow.AddMinutes(5);

            var created = await _repo.CreateWithInfoAsync(user, ct);

            await SendOtpEmailAsync(created, ct);
            return (_mapper.Map<UserReadDto>(created), false);
        }

        // ========== UPDATE ==========
        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto, CancellationToken ct = default)
        {
            if (id != dto.UserID) return false;
            var u = await _repo.GetByIdAsync(id, ct);
            if (u == null) return false;

            // map các field đơn giản
            _mapper.Map(dto, u);

            // logic bổ sung
            if (dto.IsActive.HasValue) u.IsActive = dto.IsActive.Value;
            if (dto.CountWarning.HasValue) u.CountWarning = dto.CountWarning.Value;
            if (dto.otp_code is not null) u.otp_code = dto.otp_code;
            if (dto.otp_expires.HasValue) u.otp_expires = dto.otp_expires.Value;
            if (dto.is_verified.HasValue) u.is_verified = dto.is_verified.Value;

            if (u.CountWarning >= 5)
                u.IsActive = false;

            await _repo.UpdateAsync(u, ct);
            return true;
        }

        // ========== DELETE ==========
        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            if (u == null) return false;
            await _repo.DeleteAsync(u, ct);
            return true;
        }

        // ========== OTP RESEND ==========
        public async Task<UserReadDto?> GenerateOtpAsync(int userId, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(userId, ct);
            if (u == null) return null;

            if (!await _limiter.CanSendAsync(userId, ct))
                throw new InvalidOperationException("OTP rate limit exceeded");

            u.otp_code = GenerateOtp6();
            u.otp_expires = DateTime.UtcNow.AddMinutes(5);
            u.is_verified = false;

            var saved = await _repo.UpdateAsync(u, ct);
            await SendOtpEmailAsync(saved, ct);
            await _limiter.RecordSendAsync(userId, ct);

            return _mapper.Map<UserReadDto>(saved);
        }

        // ========== OTP VERIFY ==========
        public async Task<bool> VerifyOtpAsync(int userId, string otp_code, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(userId, ct);
            if (u == null) return false;

            if (string.IsNullOrWhiteSpace(u.otp_code) ||
                !string.Equals(u.otp_code, otp_code, StringComparison.Ordinal) ||
                u.otp_expires is null || u.otp_expires < DateTime.UtcNow)
                return false;

            u.is_verified = true;
            u.IsActive = true;
            u.otp_code = null;
            u.otp_expires = null;

            await _repo.UpdateAsync(u, ct);
            return true;
        }

        // ========== HELPERS ==========
        private static string GenerateOtp6()
        {
            var value = RandomNumberGenerator.GetInt32(0, 1_000_000);
            return value.ToString("D6");
        }

        private async Task SendOtpEmailAsync(User u, CancellationToken ct)
        {
            var subject = "Your verification code";
            var html = $@"
                <p>Hi {System.Net.WebUtility.HtmlEncode(u.FullName)},</p>
                <p>Your verification code is:</p>
                <h2 style=""letter-spacing:4px;"">{u.otp_code}</h2>
                <p>This code expires in 5 minutes.</p>
                <p>If you did not request this, please ignore.</p>";
            await _email.SendAsync(u.Email, subject, html, ct);
        }
    }
}