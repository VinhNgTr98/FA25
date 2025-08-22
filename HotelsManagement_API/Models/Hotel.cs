using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagement_API.Models
{
    public class Hotel
    {
        [Key]
        public Guid HotelId { get; set; } = Guid.NewGuid();

        [Required]
        public int UserID { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public string HotelStatus { get; set; } // Active, Inactive, Pending

        public string? Tiles { get; set; }
        public string Location { get; set; }
        public string? CheckInPolicy { get; set; }
        public string? CancellationPolicy { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
