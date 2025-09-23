using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomManagement_API.Models
{
    public enum RoomStatus { Available, Booked, Maintenance }
    [Table("Rooms")]
    public class Room
    {
        [Key]
        public Guid RoomId { get; set; }

        [Required, MaxLength(100)]
        public string RoomName { get; set; } = default!;

        [Required]
        public String RoomType { get; set; }             

        [MaxLength(500)]
        public string? Amenities { get; set; }

        [Required, MaxLength(50)]
        public string RoomStatus { get; set; } = "Available"; // Available|Booked|Maintenance

        [Required]
        public int RoomCapacity { get; set; }

        [Required]
        public Guid HotelId { get; set; }              // FK -> Hotels.HotelId

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
    }
}
