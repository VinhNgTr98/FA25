using System.ComponentModel.DataAnnotations;

namespace ImageManagement_API.Models
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }  // Primary Key, auto-increment

        public bool IsHotelImg { get; set; } = false; // default 0
        public bool IsRoomImg { get; set; } = false;  // default 0
        public bool IsTourImg { get; set; } = false;  // default 0
        public bool IsVehicleImage { get; set; } = false; // default 0
        public bool IsTourAgencyImg { get; set; } = false;  // default 0
        public bool IsVehicleAgencyImage { get; set; } = false; // default 0
        public Guid LinkedId { get; set; } // ID của sản phẩm chứa hình ảnh này

        public string ImageUrl { get; set; } = string.Empty; // varchar(255), not null

        public string? Caption { get; set; } // nvarchar(255), nullable

        public bool IsCover { get; set; } = false; // default 0

        public DateTime UploadedAt { get; set; } = DateTime.Now; // default GETDATE()
    }
}
