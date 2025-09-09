using CartManagement_Api.Models;

namespace CartManagement_Api.DTOs
{
    public class CartItemReadDto
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public CartItemType ItemType { get; set; }
        public Guid ItemID { get; set; }
        public int Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
