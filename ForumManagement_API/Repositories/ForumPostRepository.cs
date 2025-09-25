using ForumPostManagement_API.Data;
using ForumPostManagement_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumPostManagement_API.Repositories
{
    public class ForumPostRepository : IForumPostRepository
    {
        private readonly ForumPostManagement_APIContext _db;

        public ForumPostRepository(ForumPostManagement_APIContext db)
        {
            _db = db;
        }

        public Task<ForumPost?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.ForumPost.FirstOrDefaultAsync(p => p.ForumPostId == id, ct);

        public Task<List<ForumPost>> GetAllAsync(CancellationToken ct = default)
            => _db.ForumPost
                .OrderByDescending(p => p.CreatedAtUtc)
                .ToListAsync(ct);

        public async Task AddAsync(ForumPost entity, CancellationToken ct = default)
        {
            entity.CreatedAtUtc = DateTime.UtcNow;
            entity.UpdatedAtUtc = entity.CreatedAtUtc;
            await _db.ForumPost.AddAsync(entity, ct);
        }

        public Task UpdateAsync(ForumPost entity, CancellationToken ct = default)
        {
            entity.UpdatedAtUtc = DateTime.UtcNow;
            _db.ForumPost.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ForumPost entity, CancellationToken ct = default)
        {
            _db.ForumPost.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsSlugAsync(string type, Guid linkerId, string slug, CancellationToken ct = default)
            => _db.ForumPost.AnyAsync(p => p.Type == type && p.LinkerId == linkerId && p.Slug == slug, ct);

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        public Task<List<ForumPost>> GetByTypeAsync(string type, CancellationToken ct = default)
            => _db.ForumPost.AsNoTracking()
                .Where(p => p.Type == type && p.VisibilityStatus == "Visible")
                .OrderByDescending(p => p.CreatedAtUtc)
                .ToListAsync(ct);

        public Task<List<ForumPost>> GetByContentAsync(string keyword, CancellationToken ct = default)
        {
            var k = (keyword ?? string.Empty).Trim();
            return _db.ForumPost.AsNoTracking()
                .Where(p => p.VisibilityStatus == "Visible" && p.Content.Contains(k))
                .OrderByDescending(p => p.CreatedAtUtc)
                .ToListAsync(ct);
        }
    }
}