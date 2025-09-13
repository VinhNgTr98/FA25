using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class ChangePasswordConfirmDto
    {
        [Required]
        public string OldPassword { get; set; } = default!;

        [Required]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters")]
        public string NewPassword { get; set; } = default!;

        [Required]
        [MinLength(8)]
        public string ConfirmNewPassword { get; set; } = default!;

        [Required]
        [MaxLength(6)]
        public string OtpCode { get; set; } = default!;
    }
}
