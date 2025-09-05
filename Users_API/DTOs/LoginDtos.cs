using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "UsersName is required")]
        [EmailAddress(ErrorMessage = "Invalid UsersName/Email address")]
        public string UsersName { get; set; } = default!;


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = default!;
    }
        
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string Message { get; internal set; }
    }
}
