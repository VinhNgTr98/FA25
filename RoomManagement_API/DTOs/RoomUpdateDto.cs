using System.ComponentModel.DataAnnotations;

namespace RoomManagement_API.DTOs
{
    public class RoomUpdateDto
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required(ErrorMessage = "Room name is required")]
        [StringLength(100, ErrorMessage = "Room name cannot exceed 100 characters")]
        public string RoomName { get; set; } = default!;

        [Required(ErrorMessage = "Room type is required")]
        [Range(0, 5, ErrorMessage = "Invalid room type value")]
        public int RoomType { get; set; }

        [StringLength(500, ErrorMessage = "Amenities cannot exceed 500 characters")]
        public string Amenities { get; set; }

        [Required(ErrorMessage = "Room status is required")]
        [RegularExpression("^(Available|Booked|Maintenance)$",
        ErrorMessage = "Invalid room status. Allowed values: Available, Booked, Maintenance")]
        public string RoomStatus { get; set; } = "Available";

        [Required(ErrorMessage = "Room capacity is required")]
        [Range(1, 50, ErrorMessage = "Room capacity must be between 1 and 50")]
        public int RoomCapacity { get; set; }

        //[Required(ErrorMessage = "Hotel ID is required")]
        //public Guid HotelId { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
