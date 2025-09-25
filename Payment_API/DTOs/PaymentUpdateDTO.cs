namespace Payment_API.DTOs
{
    public class PaymentUpdateDTO
    {
        public string TransactionNo { get; set; }
        public string BankCode { get; set; }
        public string Status { get; set; }
    }
}
