using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class UserCreateDto
    {
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = default!;

        [Required, MaxLength(255)]
        public string Password { get; set; } = default!;  // nhận plain-text, service sẽ hash

        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        public bool IsHotelOwner { get; set; } = false;
        public bool IsTourAgency { get; set; } = false;
        public bool IsVehicleAgency { get; set; } = false;
        public bool IsWebAdmin { get; set; } = false;
        public bool IsModerator { get; set; } = false;

        [MaxLength(10)]
        public string? otp_code { get; set; }
        public DateTime? otp_expires { get; set; }
    }

}
