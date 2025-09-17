namespace CartManagement_Api.DTOs
{
    public class CartCreateDto
    {
        public int CartID { get; set; }
        public int TotalDistinctItems { get; set; }
        public int TotalQuantity { get; set; }
        public int UserID { get; internal set; }
    }
}