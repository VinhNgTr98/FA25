using System.ComponentModel.DataAnnotations;

namespace CarManagement_API.Models
{
    public class Car
    {
        // Khóa chính
        [Key]
        public Guid CarId { get; set; }

        // ID của agency cho thuê xe
        public Guid VehicleAgencyId { get; set; }

        // Tên xe
        [Required, MaxLength(100)]
        public string CarName { get; set; } = default!;

        // Hãng xe (ví dụ: Toyota, Honda, v.v.)
        [Required, MaxLength(50)]
        public string CarBrand { get; set; } = default!;

        // Loại xe (Sedan, SUV, Pickup, v.v.)
        [Required, MaxLength(50)]
        public string CarType { get; set; } = default!;

        // Hộp số (Automatic, Manual)
        [Required, MaxLength(20)]
        public string Transmission { get; set; } = default!;

        // Loại động cơ (Gasoline, Diesel, Electric)
        [Required, MaxLength(20)]
        public string Engine { get; set; } = default!;

        // Dung tích động cơ (cc)
        public int? EngineCC { get; set; }

        // Số chỗ ngồi 
        [Range(1, 50, ErrorMessage = "Số chỗ ngồi phải lớn hơn 0 và nhỏ hơn hoặc bằng 50.")]
        public int? SeatingCapacity { get; set; }

        // Biển số xe
        [MaxLength(20)]
        public string LicensePlate { get; set; }

        // Trạng thái xe 
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Ready";

        // Giá thuê theo ngày
        [Range(0, double.MaxValue)]
        public decimal? DailyPrice { get; set; }

        // Giá thuê theo giờ
        [Range(0, double.MaxValue)]
        public decimal? HourlyPrice { get; set; }

        // Mô tả về xe
        [MaxLength(2000)]
        public string Description { get; set; }
    }
}
