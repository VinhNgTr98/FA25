namespace OrderManagement_API.DTOs
{
    public class OrderItemUpdateDto
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
