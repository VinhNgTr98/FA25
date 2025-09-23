using System.ComponentModel.DataAnnotations;

namespace TourManagement.DTOs
{
    public class TourGuideCreateDTO
    {
        [Required]
        public Guid AgencyID { get; set; }

        [Required]
        public string TourGuideName { get; set; } = string.Empty;
        public int TourGuideIdNumber { get; set; }
        public string? Description { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
