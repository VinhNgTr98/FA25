namespace ImageManagement_API.DTOs
{
    public class ImageCreateWithFilesDTO
    {
        public IFormFileCollection Files { get; set; } = default!;
        public bool IsHotelImg { get; set; }
        public bool IsRoomImg { get; set; }
        public bool IsTourImg { get; set; }
        public bool IsVehicleImage { get; set; }
        public Guid LinkedId { get; set; }
        public string? Caption { get; set; }
        public bool IsCover { get; set; }
    }
}
