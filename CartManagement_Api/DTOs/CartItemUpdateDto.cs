namespace CartManagement_Api.DTOs
{
    public class CartItemUpdateDto
    {
        public int? Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RowVersion { get; set; }
    }
}