using UsersInfoManagement_API.Models;

namespace UsersInfoManagement_API.Repositories
{
    public interface IUsersInfoRepository
    {
        Task<IEnumerable<UsersInfo>> GetAllAsync(CancellationToken ct = default);
        Task<UsersInfo?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<UsersInfo?> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken ct = default);
        Task<bool> UserExistsAsync(int userId, CancellationToken ct = default);

        Task<UsersInfo> AddAsync(UsersInfo entity, CancellationToken ct = default);
        Task UpdateAsync(UsersInfo entity, CancellationToken ct = default);
        Task DeleteAsync(UsersInfo entity, CancellationToken ct = default);
    }
}
