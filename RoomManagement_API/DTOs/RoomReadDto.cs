namespace RoomManagement_API.DTOs
{
    public class RoomReadDto
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = default!;
        public string RoomType { get; set; }
        public string Amenities { get; set; }
        public string RoomStatus { get; set; } = default!;
        public int? NumberOfBedrooms { get; set; }
        public int TotalRooms { get; set; }
        public int RoomCapacity { get; set; }
        public Guid HotelId { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
    
}
