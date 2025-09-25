using AutoMapper;
using User_API.DTOs;
using User_API.Models;
using User_API.Repositories;
using UserManagement_API.DTOs;
using System.Security.Cryptography;

namespace UserManagement_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IEmailSender _email;
        private readonly IOtpLimiter _limiter;
        public UserService(IUserRepository repo, IEmailSender email, IOtpLimiter limiter)
        {
            _repo = repo;
            _email = email;
            _limiter = limiter;
        }

        private static UserReadDto MapToReadDto(User u) => new()
        {
            UserID = u.UserID,
            Email = u.Email,
            FullName = u.FullName,
            Password = u.PasswordHash,
            IsHotelOwner = u.IsHotelOwner,
            IsTourAgency = u.IsTourAgency,
            IsVehicleAgency = u.IsVehicleAgency,
            IsWebAdmin = u.IsWebAdmin,
            IsModerator = u.IsModerator,
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

        public async Task<UserReadDto?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var u = await _repo.GetByEmailAsync(email, ct);
            return u == null ? null : MapToReadDto(u);
        }

        public async Task<(UserReadDto User, bool IsResend)> CreateWithInfoAsync(
            UserWithInfoCreateDto dto, CancellationToken ct = default)
        {
            var existing = await _repo.GetByEmailAsync(dto.Email, ct);
            if (existing != null)
            {
                if (existing.is_verified)
                    throw new InvalidOperationException("EMAIL_EXISTS");

                if (!await _limiter.CanSendAsync(existing.UserID, ct))
                    throw new InvalidOperationException("OTP_RATE_LIMIT");

                existing.otp_code = GenerateOtp6();
                existing.otp_expires = DateTime.UtcNow.AddMinutes(5);
                existing.is_verified = false;

                var saved = await _repo.UpdateAsync(existing, ct);

                var subject = "Your verification code";
                var html = $@"
                <p>Hi {System.Net.WebUtility.HtmlEncode(saved.FullName)},</p>
                <p>Your verification code is:</p>
                <h2 style=""letter-spacing:4px;"">{saved.otp_code}</h2>
                <p>This code expires in 5 minutes.</p>
                <p>If you did not request this, please ignore.</p>";

                await _email.SendAsync(saved.Email, subject, html, ct);
                await _limiter.RecordSendAsync(saved.UserID, ct);

                return (MapToReadDto(saved), true);
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new InvalidOperationException("PASSWORD_REQUIRED");

            if (!ValidateNewPassword(dto.Password))
                throw new InvalidOperationException("PASSWORD_POLICY");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var u = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = passwordHash,
                is_verified = false,
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
                IsHotelOwner = dto.IsHotelOwner,
                IsTourAgency = dto.IsTourAgency,
                IsVehicleAgency = dto.IsVehicleAgency,
                IsWebAdmin = dto.IsWebAdmin,
                IsModerator = dto.IsModerator,
                UsersInfo = new UsersInfo
                {
                    FullName = dto.FullName,
                    DateOfBirth = dto.DateOfBirth,
                    ProfilePictureUrl = dto.ProfilePictureUrl,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address
                }
            };

            u.otp_code = GenerateOtp6();
            u.otp_expires = DateTime.UtcNow.AddMinutes(5);

            var created = await _repo.CreateWithInfoAsync(u, ct);

            var subjectNew = "Your verification code";
            var htmlNew = $@"
            <p>Hi {System.Net.WebUtility.HtmlEncode(created.FullName)},</p>
            <p>Your verification code is:</p>
            <h2 style=""letter-spacing:4px;"">{created.otp_code}</h2>
            <p>This code expires in 5 minutes.</p>
            <p>If you did not request this, please ignore.</p>";

            await _email.SendAsync(created.Email, subjectNew, htmlNew, ct);

            return (MapToReadDto(created), false);
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto, CancellationToken ct = default)
        {
            if (id != dto.UserID) return false;

            var u = await _repo.GetByIdAsync(id, ct);
            if (u == null) return false;

            u.Email = dto.Email;
            u.FullName = dto.FullName;
            u.IsHotelOwner = dto.IsHotelOwner;
            u.IsTourAgency = dto.IsTourAgency;
            u.IsVehicleAgency = dto.IsVehicleAgency;
            u.IsWebAdmin = dto.IsWebAdmin;
            u.IsModerator = dto.IsModerator;

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

            if (!await _limiter.CanSendAsync(userId, ct))
                throw new InvalidOperationException("OTP rate limit exceeded");

            u.otp_code = GenerateOtp6();
            u.otp_expires = DateTime.UtcNow.AddMinutes(5);
            u.is_verified = false;

            var saved = await _repo.UpdateAsync(u, ct);

            var subject = "Your verification code";
            var html = $@"
                <p>Hi {System.Net.WebUtility.HtmlEncode(saved.FullName)},</p>
                <p>Your verification code is:</p>
                <h2 style=""letter-spacing:4px;"">{saved.otp_code}</h2>
                <p>This code expires in 5 minutes.</p>
                <p>If you did not request this, please ignore.</p>";

            await _email.SendAsync(saved.Email, subject, html, ct);
            await _limiter.RecordSendAsync(userId, ct);

            return MapToReadDto(saved);
        }

        private static string GenerateOtp6()
        {
            var value = RandomNumberGenerator.GetInt32(0, 1_000_000);
            return value.ToString("D6");
        }

        public async Task<bool> VerifyOtpAsync(int userId, string otp_code, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(userId, ct);
            if (u == null) return false;

            if (string.IsNullOrWhiteSpace(u.otp_code) ||
                !string.Equals(u.otp_code, otp_code, StringComparison.Ordinal) ||
                u.otp_expires is null || u.otp_expires < DateTime.UtcNow)
            {
                return false;
            }

            u.is_verified = true;
            u.IsActive = true;
            u.otp_code = null;
            u.otp_expires = null;

            await _repo.UpdateAsync(u, ct);
            return true;
        }

        public async Task<bool> RequestChangePasswordAsync(int userId, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(userId, ct);
            if (u == null) return false;

            // Rate limit OTP
            if (!await _limiter.CanSendAsync(userId, ct))
                throw new InvalidOperationException("OTP_RATE_LIMIT");

            u.otp_code = GenerateOtp6();
            u.otp_expires = DateTime.UtcNow.AddMinutes(5);

            await _repo.UpdateAsync(u, ct);

            var subject = "Password change verification code";
            var html = $@"
                <p>Hi {System.Net.WebUtility.HtmlEncode(u.FullName)},</p>
                <p>You requested to change your password.</p>
                <p>Your OTP code is:</p>
                <h2 style=""letter-spacing:4px;"">{u.otp_code}</h2>
                <p>This code expires in 5 minutes.</p>
                <p>If you did not request this, please ignore.</p>";

            await _email.SendAsync(u.Email, subject, html, ct);
            await _limiter.RecordSendAsync(userId, ct);
            return true;
        }

        

        private bool ValidateNewPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            // Ít nhất: 1 chữ hoa, 1 chữ thường, 1 số, 1 ký tự đặc biệt.
            var hasUpper = password.Any(char.IsUpper);
            var hasLower = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch));
            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        public async Task<bool> ConfirmChangePasswordAsync(int userId, string oldPassword, string newPassword, CancellationToken ct = default)
        {
            var u = await _repo.GetByIdAsync(userId, ct);
            if (u == null) return false;

            // Kiểm tra mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, u.PasswordHash))
                return false;

            // Kiểm tra policy mật khẩu mới
            if (!ValidateNewPassword(newPassword))
                return false;

            // Cập nhật mật khẩu
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _repo.UpdateAsync(u, ct);
            return true;
        }
        public async Task<bool> UpdateSingleRoleAsync(int userId, UpdateUserRoleDto dto, CancellationToken ct = default)
        {
            var user = await _repo.GetByIdAsync(userId, ct);
            if (user == null) return false;

            var candidates = new List<(string name, bool? value)>
            {
                ("HotelOwner", dto.IsHotelOwner),
                ("TourAgency", dto.IsTourAgency),
                ("VehicleAgency", dto.IsVehicleAgency)
            };
            var selected = candidates.Where(x => x.value.HasValue).ToList();
            if (selected.Count != 1)
                throw new ArgumentException("Phải gửi đúng 1 role mỗi lần (IsHotelOwner / IsTourAgency / IsVehicleAgency).");

            var (roleName, desired) = (selected[0].name, selected[0].value!.Value);

            if (!desired && string.IsNullOrWhiteSpace(dto.RejectedNote))
                throw new ArgumentException($"Tắt/Từ chối {roleName} cần RejectedNote.");

            switch (roleName)
            {
                case "HotelOwner":
                    {
                        if (desired)
                        {
                            if (!user.IsHotelOwner) user.IsHotelOwner = true;
                            user.RejectedNote = null;
                        }
                        else
                        {
                            if (user.IsHotelOwner) user.IsHotelOwner = false;
                            user.RejectedNote = dto.RejectedNote!.Trim();
                        }
                        break;
                    }
                case "TourAgency":
                    {
                        if (desired)
                        {
                            if (!user.IsTourAgency) user.IsTourAgency = true;
                            user.RejectedNote = null;
                        }
                        else
                        {
                            if (user.IsTourAgency) user.IsTourAgency = false;
                            user.RejectedNote = dto.RejectedNote!.Trim();
                        }
                        break;
                    }
                case "VehicleAgency":
                    {
                        if (desired)
                        {
                            if (!user.IsVehicleAgency) user.IsVehicleAgency = true;
                            user.RejectedNote = null;
                        }
                        else
                        {
                            if (user.IsVehicleAgency) user.IsVehicleAgency = false;
                            user.RejectedNote = dto.RejectedNote!.Trim();
                        }
                        break;
                    }
                default:
                    throw new ArgumentException("Role không hợp lệ.");
            }

            await _repo.UpdateAsync(user, ct);
            return true;
        }

    }


}