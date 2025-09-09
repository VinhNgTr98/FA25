namespace CartManagement_Api.Models
{
    public enum CartItemType
    {
        Room = 1,
        Tour = 2,
        Vehicle = 3,
        Service = 4
    }
    public class CartItem
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }

        public CartItemType ItemType { get; set; }                 // lưu enum, map ra string
        public Guid ItemID { get; set; }                           // uniqueidentifier
        public int Quantity { get; set; }                          // not null

        public DateTime? StartDate { get; set; }                   // cho Room/Vehicle
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Cart Cart { get; set; }                             // navigation về Cart
    }
}
