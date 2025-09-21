namespace BookingManagement_API.DTOs
{
    public class BookingUpdateDto
    {
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public decimal? TaxAmount { get; set; }
        public int? CouponId { get; set; }
        public string? BookingNote { get; set; }
    }
}