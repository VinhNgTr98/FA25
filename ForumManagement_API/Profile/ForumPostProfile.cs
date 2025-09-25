using AutoMapper;
using ForumPostManagement_API.DTOs;
using ForumPostManagement_API.Models;
using System.Text.RegularExpressions;

namespace ForumPostManagement_API.Mapping
{
    public class ForumPostProfile : Profile
    {
        private const char TagSeparator = ',';

        public ForumPostProfile()
        {
            // Model -> Read DTO
            CreateMap<ForumPost, ForumPostReadDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.TagsCsv)
                        ? Array.Empty<string>()
                        : src.TagsCsv.Split(TagSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)));

            // Create DTO -> Model
            CreateMap<ForumPostCreateDto, ForumPost>()
                .ForMember(d => d.ForumPostId, opt => opt.Ignore())
                .ForMember(d => d.TagsCsv, opt => opt.MapFrom(src =>
                    src.Tags == null
                        ? null
                        : string.Join(TagSeparator, NormalizeTags(src.Tags))))
                // Slug sẽ generate bên Service (để kiểm tra trùng), tạm thời ignore
                .ForMember(d => d.Slug, opt => opt.Ignore())
                // Nếu bạn bỏ workflow duyệt: set Approved luôn:
                //.ForMember(d => d.ApprovalStatus, opt => opt.MapFrom(_ => "Approved"))
                .ForMember(d => d.ApprovalStatus, opt => opt.Ignore())
                .ForMember(d => d.VisibilityStatus, opt => opt.MapFrom(src => src.VisibilityStatus ?? "Visible"))
                .ForMember(d => d.UpdatedAtUtc, opt => opt.Ignore())
                .ForMember(d => d.CreatedAtUtc, opt => opt.Ignore());

            // Update DTO -> Model
            CreateMap<ForumPostUpdateDto, ForumPost>()
                .ForMember(d => d.TagsCsv, opt => opt.MapFrom(src =>
                    src.Tags == null
                        ? null
                        : string.Join(TagSeparator, NormalizeTags(src.Tags))))
                // Không tự đổi Slug ở đây (nếu cần đổi slug khi sửa tiêu đề thì xử lý ở service)
                .ForMember(d => d.Slug, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAtUtc, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.VisibilityStatus, opt =>
                    opt.Condition(src => src.VisibilityStatus != null))
                .ForMember(d => d.VisibilityStatus, opt =>
                    opt.MapFrom(src => src.VisibilityStatus!))
                .ForMember(d => d.Star, opt =>
                    opt.Condition(src => src.Star.HasValue))
                .ForMember(d => d.Star, opt =>
                    opt.MapFrom(src => src.Star!.Value));
        }

        private static IEnumerable<string> NormalizeTags(IEnumerable<string> tags)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var raw in tags)
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;
                var trimmed = raw.Trim();
                // Loại bỏ khoảng trắng thừa giữa các chữ (optional)
                trimmed = Regex.Replace(trimmed, "\\s{2,}", " ");
                if (seen.Add(trimmed))
                    yield return trimmed;
            }
        }
    }
}