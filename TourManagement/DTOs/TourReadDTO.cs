using System.ComponentModel.DataAnnotations;

namespace TourManagement.DTOs
{
    public class TourReadDTO
    {
        [Key]
        public Guid TourID { get; set; }
        public Guid TourAgencyID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Itinerary { get; set; }
        public string? Description { get; set; }
        public int Length { get; set; }
        public string Transportation { get; set; }
        public string? Resident { get; set; }
        public int TourCapacity { get; set; }
        public decimal Price { get; set; }
    }
}
