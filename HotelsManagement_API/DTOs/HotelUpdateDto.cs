using System.ComponentModel.DataAnnotations;

namespace HotelsManagement_API.DTOs
{
    public class HotelUpdateDto 
    {
        [Required]
        public Guid HotelId { get; set; }

        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int Category { get; set; }

        [Required(ErrorMessage = "HotelStatus is required")]
        [StringLength(50, ErrorMessage = "HotelStatus cannot exceed 50 characters")]
        public string HotelStatus { get; set; }

        [StringLength(200, ErrorMessage = "Tiles cannot exceed 200 characters")]
        public string? Tiles { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        public string? CheckInPolicy { get; set; }
        public string? CancellationPolicy { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}
