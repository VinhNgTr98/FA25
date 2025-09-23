namespace ImageManagement_API.DTOs
{
    public class ImageCreateWithFilesDTO
    {
        public IFormFileCollection Files { get; set; } = default!;
        public bool IsHotelImg { get; set; } = false;
        public bool IsRoomImg { get; set; } = false;
        public bool IsTourImg { get; set; } = false;
        public bool IsVehicleImage { get; set; } = false;
        public bool IsTourAgencyImg { get; set; } = false;
        public bool IsVehicleAgencyImage { get; set; } = false;
        public Guid LinkedId { get; set; }
        public bool IsCover { get; set; }
    }
}
