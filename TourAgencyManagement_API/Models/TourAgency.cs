using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgencyManagement_API.Models
{
    public class TourAgency
    {
        [Key]
        public Guid TourAgencyId { get; set; }

        [Required]
        
        public int UserID { get; set; }

        [Required, MaxLength(255)]
        public string AgencyName { get; set; } = null!;

        [Required, MaxLength(255)]
        public string Location { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(4000)]
        public string? DefaultCancellationPolicy { get; set; }

        [MaxLength(4000)]
        public string? DefaultRequirements { get; set; }

        // Default Notifications
        public bool Send24hReminderByDefault { get; set; } = true;
        public bool SendThankYouEmailAfterTour { get; set; } = false;

        // Auditing
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
