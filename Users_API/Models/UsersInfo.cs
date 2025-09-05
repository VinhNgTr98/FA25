using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User_API.Models
{
    [Table("UsersInfo")]
    public class UsersInfo
    {
        [Key]
        public int UsersInfoID { get; set; }

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

        // FK -> Users.UserID
        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User User { get; set; } = null!;
    }
}
