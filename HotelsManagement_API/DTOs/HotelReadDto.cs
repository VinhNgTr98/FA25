namespace HotelsManagement_API.DTOs
{
    public class HotelReadDto
    {
        public Guid HotelId { get; set; }
        public int UserID { get; set; }
        public int Category { get; set; }
        public string HotelStatus { get; set; }
        public string? HotelName { get; set; }
        public string Location { get; set; }
        public string? CheckInPolicy { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
    }
}
