using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class UserWithInfoCreateDto
    {
        // User
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = default!;
        [Required, MaxLength(255)]
        public string Password { get; set; } = default!;

        // Roles
        public bool IsHotelOwner { get; set; } = false;
        public bool IsTourAgency { get; set; } = false;
        public bool IsVehicleAgency { get; set; } = false;
        public bool IsWebAdmin { get; set; } = false;
        public bool IsSupervisor { get; set; } = false;

        // UsersInfo
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        [MaxLength(255)]
        public string? ProfilePictureUrl { get; set; }
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        [MaxLength(255)]
        public string? Address { get; set; }
    }

}
