using System.ComponentModel.DataAnnotations;

namespace ForumPostManagement_API.Models
{
    public class ForumPost
    {
        public Guid ForumPostId { get; set; }

        public int UserID { get; set; }

        public Guid LinkerId { get; set; }

        // "Hotel" | "Tour" | "Vehicle"
        [Required, MaxLength(30)]
        public string Type { get; set; } = null!;

        [Required, MaxLength(180)]
        public string Title { get; set; } = null!;

        [MaxLength(350)]
        public string? Summary { get; set; }

        [MaxLength(400)]
        public string? TagsCsv { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required, MaxLength(160)]
        public string Slug { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        // Trạng thái duyệt của bài viết (Pending, Approved, Rejected)
        [Required]
        [MaxLength(20)]
        public string ApprovalStatus { get; set; } = "Pending"; // Mặc định là Pending

        // Lý do khi Rejected (yêu cầu khi chuyển sang Rejected)
        [MaxLength(400)]
        public string? RejectedlNote { get; set; }

        // Trạng thái ẩn / hiển thị của bài viết (Visible, Hidden)
        [Required]
        [MaxLength(20)]
        public string VisibilityStatus { get; set; } = "Visible"; // Mặc định là Visible

        public int Star { get; set; } = 0; // Mặc định là 1 sao
    }
}
