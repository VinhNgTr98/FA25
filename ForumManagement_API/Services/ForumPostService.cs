using AutoMapper;
using ForumPostManagement_API.DTOs;
using ForumPostManagement_API.Models;
using ForumPostManagement_API.Repositories;
using System.Text;
using System.Text.RegularExpressions;

namespace ForumPostManagement_API.Services
{
    public class ForumPostService : IForumPostService
    {
        private readonly IForumPostRepository _repo;
        private readonly IMapper _mapper;

        // Regex cho việc tạo slug
        private static readonly Regex MultiDash = new("\\-+", RegexOptions.Compiled);
        private static readonly Regex NonValid = new("[^a-z0-9\\-]", RegexOptions.Compiled);

        public ForumPostService(IForumPostRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ForumPostReadDto> CreateAsync(ForumPostCreateDto dto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<ForumPost>(dto);

            // Sinh slug duy nhất
            entity.Slug = await GenerateUniqueSlugAsync(dto.Title, dto.Type, dto.LinkerId, ct);

            // Bạn có thể auto-approve hoặc giữ Pending tùy nhu cầu:
            if (entity.ApprovalStatus is null or "")
                entity.ApprovalStatus = "Approved";

            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);

            return _mapper.Map<ForumPostReadDto>(entity);
        }

        public async Task<ForumPostReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return entity == null ? null : _mapper.Map<ForumPostReadDto>(entity);
        }

        public async Task<IReadOnlyList<ForumPostReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(_mapper.Map<ForumPostReadDto>).ToList();
        }

        public async Task<ForumPostReadDto?> UpdateAsync(Guid id, ForumPostUpdateDto dto, bool regenerateSlugIfTitleChanged = false, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;

            var oldTitle = entity.Title;

            _mapper.Map(dto, entity);

            if (regenerateSlugIfTitleChanged && !string.Equals(oldTitle, entity.Title, StringComparison.Ordinal))
            {
                entity.Slug = await GenerateUniqueSlugAsync(entity.Title, entity.Type, entity.LinkerId, ct);
            }

            await _repo.UpdateAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);

            return _mapper.Map<ForumPostReadDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return false;
            await _repo.DeleteAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IReadOnlyList<ForumPostReadDto>> GetByTypeAsync(string type, CancellationToken ct = default)
        {
            var list = await _repo.GetByTypeAsync(type, ct);
            return list.Select(_mapper.Map<ForumPostReadDto>).ToList();
        }

        public async Task<IReadOnlyList<ForumPostReadDto>> GetByContentAsync(string keyword, CancellationToken ct = default)
        {
            var list = await _repo.GetByContentAsync(keyword, ct);
            return list.Select(_mapper.Map<ForumPostReadDto>).ToList();
        }
        public async Task<ForumPostReadDto?> ChangeApprovalStatusAsync(Guid id, ChangeApprovalStatusDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;

            // Chỉ cho phép đổi từ Pending (nếu bạn muốn cho đổi từ bất kỳ trạng thái thì bỏ điều kiện này)
            if (!string.Equals(entity.ApprovalStatus, "Pending", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Chỉ được đổi trạng thái khi bài viết đang ở Pending.");

            var status = dto.Status?.Trim();
            if (!string.Equals(status, "Approved", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(status, "Rejected", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Status phải là 'Approved' hoặc 'Rejected'.");
            }

            if (string.Equals(status, "Approved", StringComparison.OrdinalIgnoreCase))
            {
                // Approved: không cần note
                entity.ApprovalStatus = "Approved";
                // Nếu model có ApprovalNote thì để null, nếu không có thì bỏ qua
                // entity.ApprovalNote = null;
            }
            else
            {
                // Rejected: bắt buộc note
                if (string.IsNullOrWhiteSpace(dto.Note))
                    throw new ArgumentException("Khi Rejected, bắt buộc phải kèm lý do (note).");

                entity.ApprovalStatus = "Rejected";
                // Nếu model có ApprovalNote: lưu lý do vào đó
                // entity.ApprovalNote = dto.Note!.Trim();
            }

            entity.UpdatedAtUtc = DateTime.UtcNow;
            await _repo.UpdateAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);

            return _mapper.Map<ForumPostReadDto>(entity);
        }

        // ================= Helpers =================

        private async Task<string> GenerateUniqueSlugAsync(string rawTitle, string type, Guid linkerId, CancellationToken ct)
        {
            var baseSlug = Slugify(rawTitle);
            if (string.IsNullOrWhiteSpace(baseSlug))
                baseSlug = Guid.NewGuid().ToString("n")[..8];

            var slug = baseSlug;
            int i = 2;
            while (await _repo.ExistsSlugAsync(type, linkerId, slug, ct))
            {
                slug = $"{baseSlug}-{i++}";
            }
            return slug;
        }

        private static string Slugify(string? text, int maxLength = 120)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            var lower = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in lower)
            {
                var cat = char.GetUnicodeCategory(c);
                if (cat == System.Globalization.UnicodeCategory.NonSpacingMark) continue;
                if (char.IsLetterOrDigit(c)) sb.Append(c);
                else if (char.IsWhiteSpace(c) || c == '-' || c == '_') sb.Append('-');
            }
            var slug = sb.ToString();
            slug = MultiDash.Replace(slug, "-");
            slug = NonValid.Replace(slug, "");
            slug = slug.Trim('-');
            if (slug.Length > maxLength) slug = slug[..maxLength].Trim('-');
            return slug;
        }
    }
}