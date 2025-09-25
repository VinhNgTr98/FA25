using System.ComponentModel.DataAnnotations;

namespace ForumPostManagement_API.DTOs
{
    public class ForumPostUpdateDto
    {
        [Required, MaxLength(180)]
        public string Title { get; set; } = null!;

        [MaxLength(350)]
        public string? Summary { get; set; }

        public IEnumerable<string>? Tags { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        // Cho phép thay đổi hiển thị (nếu cần ẩn bài)
        [MaxLength(20)]
        public string? VisibilityStatus { get; set; }

        // Cho phép cập nhật số sao (nếu có use case)
        public int? Star { get; set; }
    }
}
