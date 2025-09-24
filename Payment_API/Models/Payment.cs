using System.ComponentModel.DataAnnotations;

namespace Payment_API.Models
{
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }

        public Guid BookingId { get; set; }  // Liên kết tới Booking
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";

        public string PaymentMethod { get; set; } = "VNPAY";
        public string TransactionNo { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Success, Failed, Refunded

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Additional field to store payment URL
        public string PaymentUrl { get; set; } = string.Empty;
    }
}
