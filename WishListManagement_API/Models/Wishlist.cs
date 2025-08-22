using System.ComponentModel.DataAnnotations;

namespace WishListManagement_API.Models
{
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? VehiclesId { get; set; }
        public Guid? TourId { get; set; }
        public Guid? ServiceId { get; set; }
        public DateTime? AddedAt { get; set; } = DateTime.UtcNow;
    }
}
