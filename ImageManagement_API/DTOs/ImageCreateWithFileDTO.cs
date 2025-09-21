namespace ImageManagement_API.DTOs
{
    public class ImageCreateWithFileDTO
    {
        public IFormFile File { get; set; } = null!;

        public bool IsHotelImg { get; set; } = false;
        public bool IsRoomImg { get; set; } = false;
        public bool IsTourImg { get; set; } = false;
        public bool IsVehicleImage { get; set; } = false;
        public Guid LinkedId { get; set; }
        public string? Caption { get; set; }
        public bool IsCover { get; set; } = false;
    }
}
