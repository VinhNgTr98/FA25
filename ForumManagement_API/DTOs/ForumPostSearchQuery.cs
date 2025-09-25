using System.ComponentModel.DataAnnotations;

namespace ForumPostManagement_API.DTOs
{
    public class ForumPostSearchQuery
    {
        public string? Type { get; set; }
        public Guid? LinkerId { get; set; }
        public string? Keyword { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public bool TagMatchAll { get; set; } = false;
        public string? ApprovalStatus { get; set; }
        public string? VisibilityStatus { get; set; }
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public string? Sort { get; set; } = "recent";
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}
