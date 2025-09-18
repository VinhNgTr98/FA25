using System.ComponentModel.DataAnnotations;

namespace TourManagement.DTOs
{
    public class TourCreateDTO
    {
        [Required]
        public Guid AgencyId { get; set; }

        [Required, MaxLength(200)]
        public string TourName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string StartingPoint { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Destination { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Range(1, 365)]
        public int DurationDays { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, MaxLength(50)]
        public string Transportation { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int MaxCapacity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal BasePrice { get; set; }

        [Required, MaxLength(10)]
        public string Currency { get; set; } = "USD";

        public string? Policies { get; set; }
        public string[] Languages { get; set; } = Array.Empty<string>();
        public string[] Tags { get; set; } = Array.Empty<string>();

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Draft";
    }
}
