using System.ComponentModel.DataAnnotations;

namespace TourAgencyManagement_API.DTOs
{
    public class TourAgencyReadDTO
    {
        [Key]
        public Guid TourAgencyId { get; set; }
        public int UserID { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
