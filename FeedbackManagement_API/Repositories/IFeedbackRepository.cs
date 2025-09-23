using FeedbackManagement_API.Model;

namespace FeedbackManagement_API.Repositories
{
    public interface IFeedbackRepository
    {
        Task<Feedbacks?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<(IReadOnlyList<Feedbacks> items, long total)> GetPagedByLinkedAsync(
            string linkedType, Guid linkedId, int page, int pageSize, string? sort, CancellationToken ct = default);

        Task<IDictionary<int, int>> GetRatingDistributionAsync(string linkedType, Guid linkedId, CancellationToken ct = default);

        Task<double?> GetAverageRatingAsync(string linkedType, Guid linkedId, CancellationToken ct = default);

        Task AddAsync(Feedbacks entity, CancellationToken ct = default);
        Task UpdateAsync(Feedbacks entity, CancellationToken ct = default);
        Task DeleteAsync(Feedbacks entity, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);

        // NEW: Generic gets (paged)
        Task<(IReadOnlyList<Feedbacks> items, long total)> GetAllPagedAsync(int page, int pageSize, CancellationToken ct = default);
        Task<(IReadOnlyList<Feedbacks> items, long total)> GetByLinkedTypePagedAsync(string linkedType, int page, int pageSize, CancellationToken ct = default);
        Task<(IReadOnlyList<Feedbacks> items, long total)> GetByLinkedIdPagedAsync(Guid linkedId, int page, int pageSize, CancellationToken ct = default);
        Task<(IReadOnlyList<Feedbacks> items, long total)> GetByUserIDPagedAsync(int userID, int page, int pageSize, CancellationToken ct = default);
        // Chỉ lấy feedback gốc theo Rating (bỏ qua reply)
        Task<(IReadOnlyList<Feedbacks> items, long total)> GetByRatingPagedAsync(int rating, int page, int pageSize, CancellationToken ct = default);

        // Replies
        Task<IReadOnlyList<Feedbacks>> GetRepliesByFeedbackIdAsync(int feedbackId, CancellationToken ct = default);
        Task<(IReadOnlyList<Feedbacks> items, long total)> GetRepliesByFeedbackIdPagedAsync(int feedbackId, int page, int pageSize, CancellationToken ct = default);
    }
}