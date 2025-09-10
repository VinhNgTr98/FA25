namespace CartManagement_Api.DTOs
{
    public class CartReadDto
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public List<CartItemReadDto> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? RowVersion { get; set; }
    }
}