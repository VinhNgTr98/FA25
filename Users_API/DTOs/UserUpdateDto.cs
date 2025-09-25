using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public int UserID { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = default!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        public bool IsHotelOwner { get; set; }
        public bool IsTourAgency { get; set; }
        public bool IsVehicleAgency { get; set; }
        public bool IsWebAdmin { get; set; }
        public bool IsModerator { get; set; }

        public bool? IsActive { get; set; }

        [MaxLength(400)]
        public string? RejectedlNote { get; set; }
        public int? CountWarning { get; set; }  

        [MaxLength(6)]
        public string? otp_code { get; set; }
        public DateTime? otp_expires { get; set; }
        public bool? is_verified { get; set; }
    }
}