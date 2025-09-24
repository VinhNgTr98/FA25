namespace Payment_API.DTOs
{
    public class PaymentReadDTO
    {
        public Guid PaymentId { get; set; }
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionNo { get; set; }
        public string BankCode { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string PaymentUrl { get; set; }
    }
}
