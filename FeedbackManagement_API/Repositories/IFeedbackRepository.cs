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
    }
}