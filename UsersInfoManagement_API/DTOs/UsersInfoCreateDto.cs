using System.ComponentModel.DataAnnotations;

namespace UsersInfoManagement_API.Dtos.UsersInfo
{
    public class UsersInfoCreateDto
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        [MaxLength(255)]
        public string? ProfilePictureUrl { get; set; }
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = null!;
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        [MaxLength(255)]
        public string? Address { get; set; }
        [Required]
        public int UsersID { get; set; }
    }
}
