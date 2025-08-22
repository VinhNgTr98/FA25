namespace WishListManagement_API.DTOs
{
    public class WishlistDto
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }

        // 4 cột raw từ DB (giữ nguyên schema)
        public Guid? HotelId { get; set; }
        public Guid? VehiclesId { get; set; }
        public Guid? TourId { get; set; }
        public Guid? ServiceId { get; set; }

        // Trường dẫn xuất tiện dùng cho FE
        public WishlistTargetType TargetType { get; set; }
        public Guid TargetId { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
