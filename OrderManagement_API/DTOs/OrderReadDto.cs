namespace OrderManagement_API.DTOs
{
    public class OrderReadDto
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public decimal? TaxAmount { get; set; }
    }
}
