using User_API.DTOs;
using UserManagement_API.DTOs;

namespace Users_API.Services
{
    public interface IUserService
    {
        Task<UserReadDto?> GetAsync(int id, CancellationToken ct);
        Task<List<UserReadDto>> GetAllAsync(CancellationToken ct);
        Task<UserReadDto> CreateAsync(UserCreateDto dto, CancellationToken ct);
        Task<UserReadDto?> UpdateAsync(UserUpdateDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);

        // tăng cảnh báo + tự khóa nếu >=5
        Task<UserReadDto?> IncreaseWarningAsync(int userId, int increaseBy, CancellationToken ct);
        Task<UserReadDto?> GetByUsernameAsync(string username, CancellationToken ct);
    }

}
