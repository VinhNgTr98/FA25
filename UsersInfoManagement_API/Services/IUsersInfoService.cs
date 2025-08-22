using UsersInfoManagement_API.Dtos.UsersInfo;

namespace UsersInfoManagement_API.Services
{
    public interface IUsersInfoService
    {
        Task<IEnumerable<UsersInfoReadDto>> GetAllAsync(CancellationToken ct = default);
        Task<UsersInfoReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<UsersInfoReadDto?> GetByUserIdAsync(int usersId, CancellationToken ct = default);
        Task<UsersInfoReadDto> CreateAsync(UsersInfoCreateDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, UsersInfoUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
