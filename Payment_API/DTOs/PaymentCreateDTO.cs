namespace Payment_API.DTOs
{
    public class PaymentCreateDTO
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string PaymentMethod { get; set; } = "VNPAY";
    }
}
