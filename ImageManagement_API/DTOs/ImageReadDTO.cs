using System.ComponentModel.DataAnnotations;

namespace ImageManagement_API.DTOs
{
    public class ImageReadDTO
    {
        [Key]
        public int ImageID { get; set; }
        public bool IsHotelImg { get; set; }
        public bool IsRoomImg { get; set; }
        public bool IsTourImg { get; set; }
        public bool IsVehicleImage { get; set; }
        public bool IsTourAgencyImg { get; set; }
        public bool IsVehicleAgencyImage { get; set; }
        public Guid LinkedId { get; set; }
        public string ImageUrl { get; set; }
        public string? Caption { get; set; }
        public bool IsCover { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
