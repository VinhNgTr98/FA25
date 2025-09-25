namespace BookingManagement_API.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserID { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // chờ thành toán 
        public decimal? TaxAmount { get; set; }
        public int? CouponId { get; set; }
        public string? BookingNote { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // thêm updatedAt nếu Status thêm  deletenotefisnitBoking (check all có status là chờ thanh toán nếu time > 5 so vs CreatedAt của bookingid và tất cả bookingItem khổi DB)
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        // Navigation
        public ICollection<BookingItem> Items { get; set; } = new List<BookingItem>();
    }
}