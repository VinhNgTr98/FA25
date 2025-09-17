namespace CartManagement_Api.DTOs
{
    public class CartItemUpdateDto
    {
        public string ItemType { get; set; } = default!;
        public int Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ItemID { get; internal set; }
    }
}