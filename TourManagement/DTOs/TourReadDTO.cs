using System.ComponentModel.DataAnnotations;
using TourManagement.Model;

namespace TourManagement.DTOs
{
    public class TourReadDTO
    {
        [Key]
        public Guid TourID { get; set; }

        public Guid AgencyID { get; set; }

        public string TourName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public string StartingPoint { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int DurationDays { get; set; }
        public string? Description { get; set; }
        public string Transportation { get; set; } = string.Empty;

        public int MaxCapacity { get; set; }

        public decimal BasePrice { get; set; }
        public string Currency { get; set; } = "VND";

        public string? Included { get; set; }
        public string? Excluded { get; set; }
        public string? Requiments { get; set; }

        public string? Policies { get; set; }
        public string? CancelPolicies { get; set; }

        public string[] Languages { get; set; } = Array.Empty<string>();
        public string[] Tags { get; set; } = Array.Empty<string>();

        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();
        public Guid TourGuideId { get; set; }
    }
}
