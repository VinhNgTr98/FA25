using System.ComponentModel.DataAnnotations;

namespace TourManagement.DTOs
{
    public class TourGuideUpdateDTO
    {
        public Guid TourGuideId { get; set; }

        public int TourGuideIdNumber { get; set; }
        public string TourGuideName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
