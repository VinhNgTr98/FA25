using ForumPostManagement_API.DTOs;

namespace ForumPostManagement_API.Services
{
    /// <summary>
    /// Service CRUD tối giản cho ForumPost.
    /// </summary>
    public interface IForumPostService
    {
        Task<ForumPostReadDto> CreateAsync(ForumPostCreateDto dto, CancellationToken ct = default);
        Task<ForumPostReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<ForumPostReadDto>> GetAllAsync(CancellationToken ct = default);
        Task<ForumPostReadDto?> UpdateAsync(Guid id, ForumPostUpdateDto dto, bool regenerateSlugIfTitleChanged = false, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<ForumPostReadDto>> GetByTypeAsync(string type, CancellationToken ct = default);
        Task<IReadOnlyList<ForumPostReadDto>> GetByContentAsync(string keyword, CancellationToken ct = default);
        Task<ForumPostReadDto?> ChangeApprovalStatusAsync(Guid id, ChangeApprovalStatusDto dto, CancellationToken ct = default);
    }
}