using User_API.DTOs;
using User_API.Models;

namespace User_API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken ct);
        Task<List<User>> GetAllAsync(CancellationToken ct);
        Task<User> AddAsync(User user, CancellationToken ct);
        Task<User?> UpdateAsync(User user, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
    }
}
