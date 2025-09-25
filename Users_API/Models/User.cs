using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_API.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }

        // Đổi từ UsersName -> Email
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = default!;

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; } = default!;

        // ✅ Thêm trường FullName
        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        public bool IsHotelOwner { get; set; } = false;
        public bool IsTourAgency { get; set; } = false;
        public bool IsVehicleAgency { get; set; } = false;
        public bool IsWebAdmin { get; set; } = false;
        public bool IsModerator { get; set; } = false;

        // Lý do khi Rejected (yêu cầu khi chuyển sang Rejected)
        [MaxLength(400)]
        public string? RejectedNote { get; set; }
        public bool IsActive { get; set; }

        public int CountWarning { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(10)]
        public string? otp_code { get; set; }

        public DateTime? otp_expires { get; set; }

        public bool is_verified { get; set; } = false;

        public UsersInfo? UsersInfo { get; set; }
    }
}