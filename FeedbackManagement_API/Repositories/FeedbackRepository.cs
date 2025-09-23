using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FeedbackManagement_API.Data;
using FeedbackManagement_API.Model;
using Microsoft.EntityFrameworkCore;

namespace FeedbackManagement_API.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FeedbackManagement_APIContext _db;

        public FeedbackRepository(FeedbackManagement_APIContext db)
        {
            _db = db;
        }

        public async Task<Feedbacks?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _db.Feedback
                .Include(f => f.Replies)
                .FirstOrDefaultAsync(f => f.FeedbackId == id, ct);
        }

        public async Task<(IReadOnlyList<Feedbacks> items, long total)> GetPagedByLinkedAsync(
            string linkedType, Guid linkedId, int page, int pageSize, string? sort, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _db.Feedback
                .AsNoTracking()
                .Where(f => f.LinkedType == linkedType && f.LinkedId == linkedId && f.IsReply == false);

            var total = await baseQuery.LongCountAsync(ct);

            var query = baseQuery
                .Include(f => f.Replies)
                .AsSplitQuery();

            query = (sort ?? "recent").ToLower() switch
            {
                "top" => query.OrderByDescending(f => f.Rating).ThenByDescending(f => f.CreatedAt),
                _ => query.OrderByDescending(f => f.CreatedAt)
            };

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<IDictionary<int, int>> GetRatingDistributionAsync(string linkedType, Guid linkedId, CancellationToken ct = default)
        {
            var distribution = await _db.Feedback.AsNoTracking()
                .Where(f => f.LinkedType == linkedType && f.LinkedId == linkedId && !f.IsReply)
                .GroupBy(f => f.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            return Enumerable.Range(1, 5)
                .ToDictionary(r => r, r => distribution.FirstOrDefault(d => d.Rating == r)?.Count ?? 0);
        }

        public async Task<double?> GetAverageRatingAsync(string linkedType, Guid linkedId, CancellationToken ct = default)
        {
            return await _db.Feedback.AsNoTracking()
                .Where(f => f.LinkedType == linkedType && f.LinkedId == linkedId && !f.IsReply)
                .Select(f => (double?)f.Rating)
                .AverageAsync(ct);
        }

        public async Task AddAsync(Feedbacks entity, CancellationToken ct = default)
        {
            await _db.Feedback.AddAsync(entity, ct);
        }

        public Task UpdateAsync(Feedbacks entity, CancellationToken ct = default)
        {
            _db.Feedback.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Feedbacks entity, CancellationToken ct = default)
        {
            _db.Feedback.Remove(entity);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);

        // NEW: generic gets (paged)
        public async Task<(IReadOnlyList<Feedbacks> items, long total)> GetAllPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _db.Feedback.AsNoTracking();
            var total = await baseQuery.LongCountAsync(ct);

            var items = await baseQuery
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<(IReadOnlyList<Feedbacks> items, long total)> GetByLinkedTypePagedAsync(string linkedType, int page, int pageSize, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _db.Feedback.AsNoTracking()
                .Where(f => f.LinkedType == linkedType);

            var total = await baseQuery.LongCountAsync(ct);

            var items = await baseQuery
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<(IReadOnlyList<Feedbacks> items, long total)> GetByLinkedIdPagedAsync(Guid linkedId, int page, int pageSize, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _db.Feedback.AsNoTracking()
                .Where(f => f.LinkedId == linkedId);

            var total = await baseQuery.LongCountAsync(ct);

            var items = await baseQuery
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<(IReadOnlyList<Feedbacks> items, long total)> GetByUserIDPagedAsync(int userID, int page, int pageSize, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _db.Feedback.AsNoTracking()
                .Where(f => f.UserID == userID);

            var total = await baseQuery.LongCountAsync(ct);

            var items = await baseQuery
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        // Chỉ feedback gốc theo rating
        public async Task<(IReadOnlyList<Feedbacks> items, long total)> GetByRatingPagedAsync(int rating, int page, int pageSize, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _db.Feedback.AsNoTracking()
                .Where(f => !f.IsReply && f.Rating == rating);

            var total = await baseQuery.LongCountAsync(ct);

            var items = await baseQuery
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<IReadOnlyList<Feedbacks>> GetRepliesByFeedbackIdAsync(int feedbackId, CancellationToken ct = default)
        {
            return await _db.Feedback.AsNoTracking()
                .Where(f => f.ReplyId == feedbackId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<(IReadOnlyList<Feedbacks> items, long total)> GetRepliesByFeedbackIdPagedAsync(int feedbackId, int page, int pageSize, CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _db.Feedback.AsNoTracking()
                .Where(f => f.ReplyId == feedbackId);

            var total = await baseQuery.LongCountAsync(ct);

            var items = await baseQuery
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }
    }
}