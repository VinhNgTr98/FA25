using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class ChangePasswordConfirmDto
    {
        [Required]
        public string OldPassword { get; set; } = default!;

        [Required]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and include at least 1 uppercase, 1 lowercase, 1 number, and 1 special character."
    )]
        public string NewPassword { get; set; } = default!;

        [Required]
        [MinLength(8)]
        public string ConfirmNewPassword { get; set; } = default!;
    }
}
