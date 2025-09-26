using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class UserWithInfoCreateDto
    {
        // User
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = default!;

        [Required, MaxLength(255)]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and include at least 1 uppercase, 1 lowercase, 1 number, and 1 special character."
    )]
        public string Password { get; set; } = default!;

        [MaxLength(10)]
        public string? otp_code { get; set; }

        public DateTime? otp_expires { get; set; }

        // Roles
        public bool IsHotelOwner { get; set; } = false;
        public bool IsTourAgency { get; set; } = false;
        public bool IsVehicleAgency { get; set; } = false;
        public bool IsWebAdmin { get; set; } = false;
        public bool IsModerator { get; set; } = false;

        // UsersInfo
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        [MaxLength(255)]
        public string? ProfilePictureUrl { get; set; }
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        [Required]
        public string Sex { get; set; } = default!;
        [MaxLength(255)]
        public string? Address { get; set; }
    }
}