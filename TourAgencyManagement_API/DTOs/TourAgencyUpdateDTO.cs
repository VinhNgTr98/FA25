using System.ComponentModel.DataAnnotations;

namespace TourAgencyManagement_API.DTOs
{
    public class TourAgencyUpdateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(255)]
        public string AgencyName { get; set; } = null!;

        [Required, MaxLength(255)]
        public string Location { get; set; } = null!;

        [Required, MaxLength(255)]
        public string Address { get; set; } = null!;

        [MaxLength(15), Phone]
        public string? Phone { get; set; }

        [MaxLength(100), EmailAddress]
        public string? Email { get; set; }

        [MaxLength(4000)]
        public string? DefaultCancellationPolicy { get; set; }

        [MaxLength(4000)]
        public string? DefaultRequirements { get; set; }

        public bool Send24hReminderByDefault { get; set; }
        public bool SendThankYouEmailAfterTour { get; set; }
    }
}
