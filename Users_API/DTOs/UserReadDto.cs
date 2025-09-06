using System.ComponentModel.DataAnnotations;

namespace User_API.DTOs
{
    public class UserReadDto
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string Email { get; set; } = default!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        public bool IsHotelOwner { get; set; }
        public bool IsTourAgency { get; set; }
        public bool IsVehicleAgency { get; set; }
        public bool IsWebAdmin { get; set; }
        public bool IsSupervisor { get; set; }

        public bool IsActive { get; set; }
        public int CountWarning { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? otp_code { get; set; }
        public DateTime? otp_expires { get; set; }
        public bool is_verified { get; set; }
    }

}
