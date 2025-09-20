namespace BookingManagement_API.Models
{
    public class BookingItem
    {
        public int BookingItemID { get; set; }
        public int BookingId { get; set; }
        public string ItemType { get; set; } = default!;
        public Guid ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation
        public Booking? Booking { get; set; }
    }
}