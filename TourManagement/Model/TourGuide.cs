using System.ComponentModel.DataAnnotations;

namespace TourManagement.Model
{
    public class TourGuide
    {
        [Key]
        public Guid TourGuideId { get; set; }
        public Guid AgencyId { get; set; }
        public int TourGuideIdNumber { get; set; }
        public string TourGuideName { get; set; }
        public string? Description { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
