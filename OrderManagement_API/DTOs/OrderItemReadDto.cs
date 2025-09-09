namespace OrderManagement_API.DTOs
{
    public class OrderItemReadDto
    {
        public int OrderItemID { get; set; }
        public string ItemType { get; set; }
        public Guid ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
