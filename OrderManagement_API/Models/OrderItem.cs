namespace OrderManagement_API.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public string ItemType { get; set; } = default!; // Room, Tour, Vehicle, Service
        public Guid ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation
        public Order Order { get; set; }
    }
}