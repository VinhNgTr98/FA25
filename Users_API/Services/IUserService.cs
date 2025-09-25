using User_API.DTOs;
using UserManagement_API.DTOs;

namespace UserManagement_API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllAsync(CancellationToken ct = default);
        Task<UserReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(UserReadDto User, bool IsResend)> CreateWithInfoAsync(UserWithInfoCreateDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, UserUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<UserReadDto?> GenerateOtpAsync(int userId, CancellationToken ct = default);
        Task<bool> VerifyOtpAsync(int userId, string otpCode, CancellationToken ct = default);
        Task<UserReadDto?> GetByEmailAsync(string email, CancellationToken ct = default);

        Task<bool> RequestChangePasswordAsync(int userId, CancellationToken ct = default);
        Task<bool> ConfirmChangePasswordAsync(int userId, string oldPassword, string newPassword, CancellationToken ct = default);
        Task<bool> UpdateSingleRoleAsync(int userId, UpdateUserRoleDto dto, CancellationToken ct = default);
    }

}
