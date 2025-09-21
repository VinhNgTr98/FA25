using System.ComponentModel.DataAnnotations;

namespace TourAgencyManagement_API.DTOs
{
    public class TourAgencyReadDto
    {
        public Guid TourAgencyId { get; set; }
        public int UserId { get; set; }
        public string AgencyName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? DefaultCancellationPolicy { get; set; }
        public string? DefaultRequirements { get; set; }
        public bool Send24hReminderByDefault { get; set; }
        public bool SendThankYouEmailAfterTour { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
