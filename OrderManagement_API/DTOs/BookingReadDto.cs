namespace OrderManagement_API.DTOs
{
    public class BookingReadDto
    {
        public int BookingId { get; set; }
        public int UserID { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public decimal? TaxAmount { get; set; }
        public int? CouponId { get; set; }
        public string? BookingNote { get; set; }
        public ICollection<BookingItemReadDto> Items { get; set; }
    }
}