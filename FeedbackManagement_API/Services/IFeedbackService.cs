using FeedbackManagement_API.DTOs;
using FeedbackManagement_API.Model;

namespace FeedbackManagement_API.Services
{
    public interface IFeedbackService
    {
        Task<Feedbacks> CreateAsync(FeedbackCreateDTO dto, CancellationToken ct = default);
        Task<Feedbacks?> UpdateAsync(int id, FeedbackUpdateDTO dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<(IEnumerable<Feedbacks> items, long total)> GetPagedAsync(
            string linkedType, Guid linkedId, int page, int pageSize, string? sort, CancellationToken ct = default);

        Task<(double? average, IDictionary<int, int> distribution)> GetSummaryAsync(
            string linkedType, Guid linkedId, CancellationToken ct = default);

        Task<Feedbacks?> GetAsync(int id, CancellationToken ct = default);
        Task<Feedbacks> CreateReplyAsync(int parentId, FeedbackReplyDTO dto, CancellationToken ct = default);

        // NEW: generic gets (paged)
        Task<(IEnumerable<Feedbacks> items, long total)> GetAllPagedAsync(int page, int pageSize, CancellationToken ct = default);
        Task<(IEnumerable<Feedbacks> items, long total)> GetByLinkedTypePagedAsync(string linkedType, int page, int pageSize, CancellationToken ct = default);
        Task<(IEnumerable<Feedbacks> items, long total)> GetByLinkedIdPagedAsync(Guid linkedId, int page, int pageSize, CancellationToken ct = default);
        Task<(IEnumerable<Feedbacks> items, long total)> GetByUserIDPagedAsync(int userID, int page, int pageSize, CancellationToken ct = default);
        Task<(IEnumerable<Feedbacks> items, long total)> GetByRatingPagedAsync(int rating, int page, int pageSize, CancellationToken ct = default);

        // Replies
        Task<IEnumerable<Feedbacks>> GetRepliesByFeedbackIdAsync(int feedbackId, CancellationToken ct = default);
        Task<(IEnumerable<Feedbacks> items, long total)> GetRepliesByFeedbackIdPagedAsync(int feedbackId, int page, int pageSize, CancellationToken ct = default);
    }
}