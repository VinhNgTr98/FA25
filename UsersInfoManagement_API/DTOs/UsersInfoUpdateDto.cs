using System.ComponentModel.DataAnnotations;

namespace UsersInfoManagement_API.Dtos.UsersInfo
{
    public class UsersInfoUpdateDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        [MaxLength(255)]
        public string? ProfilePictureUrl { get; set; }
        [MaxLength(100), EmailAddress]
        public string? Email { get; set; }
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        [MaxLength(255)]
        public string? Address { get; set; }
        public int? UsersID { get; set; } // thường KHÔNG cho phép đổi; nếu không muốn đổi thì bỏ field này
    }
}
