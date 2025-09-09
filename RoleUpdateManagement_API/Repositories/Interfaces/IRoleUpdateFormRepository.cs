using RoleUpdateManagement_API.Models;

namespace RoleUpdateManagement_API.Repositories.Interfaces
{
    public interface IRoleUpdateFormRepository
    {
        Task<IEnumerable<RoleUpdateForm>> GetAllAsync();
        Task<RoleUpdateForm?> GetByIdAsync(Guid id);
        Task AddAsync(RoleUpdateForm entity);
        Task UpdateAsync(RoleUpdateForm entity);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
