using ForumPostManagement_API.Models;

namespace ForumPostManagement_API.Repositories
{
    public interface IForumPostRepository
    {
        Task<ForumPost?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<ForumPost>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(ForumPost entity, CancellationToken ct = default);
        Task UpdateAsync(ForumPost entity, CancellationToken ct = default);
        Task DeleteAsync(ForumPost entity, CancellationToken ct = default);

        Task<bool> ExistsSlugAsync(string type, Guid linkerId, string slug, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);

        Task<List<ForumPost>> GetByTypeAsync(string type, CancellationToken ct = default);
        Task<List<ForumPost>> GetByContentAsync(string keyword, CancellationToken ct = default);
    }
}