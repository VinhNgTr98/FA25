using System.ComponentModel.DataAnnotations;

namespace TourAgencyManagement_API.DTOs
{
    public class SendEmailRequestDto
    {
        [Required, EmailAddress]
        public string To { get; set; } = null!;

        [Required, MaxLength(255)]
        public string Subject { get; set; } = null!;

        [Required]
        public string HtmlBody { get; set; } = null!;

        public string? TextBody { get; set; }
    }
}
