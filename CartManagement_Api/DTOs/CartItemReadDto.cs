namespace CartManagement_Api.DTOs
{
    public class CartItemReadDto
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public string ItemType { get; set; } = default!;
        public Guid ItemID { get; set; }
        public int Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}