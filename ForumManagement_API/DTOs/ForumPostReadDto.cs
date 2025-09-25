namespace ForumPostManagement_API.DTOs
{
    public class ForumPostReadDto
    {
        public Guid ForumPostId { get; set; }
        public int UserID { get; set; }
        public Guid LinkerId { get; set; }
        public string Type { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Summary { get; set; }
        public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
        public string Content { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string ApprovalStatus { get; set; } = null!;
        public string? RejectedlNote { get; set; } // Thêm để phản ánh model
        public string VisibilityStatus { get; set; } = null!;
        public int Star { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }
}
