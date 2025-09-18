using FeedbackManagement_API.DTOs;
using FeedbackManagement_API.Model;
using FeedbackManagement_API.Repositories;

namespace FeedbackManagement_API.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;

        public FeedbackService(IFeedbackRepository repo)
        {
            _repo = repo;
        }

        public async Task<Feedbacks?> GetAsync(int id, CancellationToken ct = default)
        {
            return await _repo.GetByIdAsync(id, ct);
        }

        public async Task<(IEnumerable<Feedbacks> items, long total)> GetPagedAsync(
            string linkedType, Guid linkedId, int page, int pageSize, string? sort, CancellationToken ct = default)
        {
            var (items, total) = await _repo.GetPagedByLinkedAsync(linkedType, linkedId, page, pageSize, sort, ct);
            return (items, total);
        }

        public async Task<(double? average, IDictionary<int, int> distribution)> GetSummaryAsync(
            string linkedType, Guid linkedId, CancellationToken ct = default)
        {
            var avg = await _repo.GetAverageRatingAsync(linkedType, linkedId, ct);
            var dist = await _repo.GetRatingDistributionAsync(linkedType, linkedId, ct);
            return (avg, dist);
        }

        // CHỈ tạo feedback gốc: luôn ReplyId = null, IsReply = false
        public async Task<Feedbacks> CreateAsync(FeedbackCreateDTO dto, CancellationToken ct = default)
        {
            if (!dto.Rating.HasValue)
                throw new ArgumentException("Rating is required for root feedback.");

            if (dto.Rating is < 1 or > 5)
                throw new ArgumentOutOfRangeException(nameof(dto.Rating), "Rating must be between 1 and 5.");

            var entity = new Feedbacks
            {
                // Nếu model là UserID thì thay bằng UserID = dto.UserId;
                UserID = dto.UserID,
                LinkedId = dto.LinkedId,
                LinkedType = dto.LinkedType,
                IsReply = false,
                ReplyId = null,                 // ÉP null cho lần tạo đầu
                Rating = dto.Rating.Value,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<Feedbacks?> UpdateAsync(int id, FeedbackUpdateDTO dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity is null) return null;

            // Không cho sửa rating của reply
            if (!entity.IsReply && dto.Rating.HasValue)
            {
                if (dto.Rating is < 1 or > 5)
                    throw new ArgumentOutOfRangeException(nameof(dto.Rating), "Rating must be between 1 and 5.");
                entity.Rating = dto.Rating.Value;
            }

            if (dto.Comment is not null)
                entity.Comment = dto.Comment;

            await _repo.UpdateAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity is null) return false;

            await _repo.DeleteAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return true;
        }

        // Tạo reply tách riêng endpoint /api/feedback/{id}/reply
        public async Task<Feedbacks> CreateReplyAsync(int parentId, FeedbackReplyDTO dto, CancellationToken ct = default)
        {
            var parent = await _repo.GetByIdAsync(parentId, ct);
            if (parent is null)
                throw new ArgumentException("Parent feedback does not exist.", nameof(parentId));

            var entity = new Feedbacks
            {
                // Nếu model là UserID thì đổi sang UserID = dto.UserID;
                UserID = dto.UserID,                // hoặc dto.UserId nếu đã thống nhất
                LinkedId = parent.LinkedId,
                LinkedType = parent.LinkedType,
                IsReply = true,
                ReplyId = parent.FeedbackId,
                Rating = parent.Rating,             // cột int: kế thừa rating cha
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return entity;
        }
    }
}