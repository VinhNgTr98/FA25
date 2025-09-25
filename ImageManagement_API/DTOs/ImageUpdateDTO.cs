namespace ImageManagement_API.DTOs
{
    public class ImageUpdateDTO
    {
        public bool IsHotelImg { get; set; }
        public bool IsRoomImg { get; set; }
        public bool IsTourImg { get; set; }
        public bool IsVehicleImage { get; set; }
        public bool IsTourAgencyImg { get; set; }
        public bool IsLicenseImage { get; set; } = false;
        public bool IsVehicleAgencyImage { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsCover { get; set; }
    }
}
