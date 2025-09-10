namespace CartManagement_Api.DTOs
{
    public class CartSummaryDto
    {
        public int CartID { get; set; }
        public int TotalDistinctItems { get; set; }
        public int TotalQuantity { get; set; }
    }
}