using System.ComponentModel.DataAnnotations;

namespace ForumPostManagement_API.DTOs
{
    public class ForumPostCreateDto
    {
        [Required]
        public int UserID { get; set; }

        // Id của thực thể gốc (Hotel/Tour/Vehicle)
        [Required]
        public Guid LinkerId { get; set; }

        // "Hotel" | "Tour" | "Vehicle"
        [Required, MaxLength(30)]
        public string Type { get; set; } = null!;

        [Required, MaxLength(180)]
        public string Title { get; set; } = null!;

        [MaxLength(350)]
        public string? Summary { get; set; }

        // Danh sách tag (tùy chọn). Sẽ được nối thành TagsCsv.
        public IEnumerable<string>? Tags { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        // Cho phép gửi trạng thái hiển thị lúc tạo (nếu cần)
        [MaxLength(20)]
        public string? VisibilityStatus { get; set; } = "Visible";
    }
}
