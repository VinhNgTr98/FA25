namespace OrderManagement_API.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserID { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal? TaxAmount { get; set; }
        public int? CouponId { get; set; }
        public string? BookingNote { get; set; }

        // Navigation
        public ICollection<BookingItem> Items { get; set; } = new List<BookingItem>();
    }
}