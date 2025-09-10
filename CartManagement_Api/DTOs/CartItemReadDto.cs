namespace CartManagement_Api.DTOs
{
    public class CartItemReadDto
    {
        public int CartItemID { get; set; }
        public string ItemType { get; set; } = string.Empty;
        public Guid ItemID { get; set; }
        public int Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string RowVersion { get; set; } = string.Empty;
    }
}