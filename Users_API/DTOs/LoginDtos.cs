using System.ComponentModel.DataAnnotations;
using User_API.DTOs;

namespace UserManagement_API.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = default!;
    }
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string? Message { get; internal set; }
        public UserReadDto User { get; set; }
    }
}
