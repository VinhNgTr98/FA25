namespace OrderManagement_API.DTOs
{
    public class OrderUpdateDto
    {
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public decimal? TaxAmount { get; set; }
        public int? CouponId { get; set; }
        public string? OrderNote { get; set; }
    }
}
