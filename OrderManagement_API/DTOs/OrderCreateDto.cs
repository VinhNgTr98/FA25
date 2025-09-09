namespace OrderManagement_API.DTOs
{
    public class OrderCreateDto
    {
        public int UserID { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal? TaxAmount { get; set; }
        public int? CouponId { get; set; }
        public string? OrderNote { get; set; }
    }
}
