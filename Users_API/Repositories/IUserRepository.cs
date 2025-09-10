using User_API.DTOs;
using User_API.Models;

namespace User_API.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(CancellationToken ct = default);
        Task<User?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);

        Task<User> CreateAsync(User user, CancellationToken ct = default);
        Task<User> UpdateAsync(User user, CancellationToken ct = default);
        Task DeleteAsync(User user, CancellationToken ct = default);
        Task<User> CreateWithInfoAsync(User user, CancellationToken ct = default);
    }

}