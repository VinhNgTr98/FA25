using System.ComponentModel.DataAnnotations;

namespace CarManagement_API.DTOs
{
    public class CarUpdateDto
    {
        [Required]
        public Guid VehicleAgencyId { get; set; }

        [Required, MaxLength(100)]
        public string CarName { get; set; }

        [Required, MaxLength(50)]
        public string CarBrand { get; set; }

        [Required, MaxLength(50)]
        public string CarType { get; set; }

        [Required, MaxLength(20)]
        public string Transmission { get; set; }

        [Required, MaxLength(20)]
        public string Engine { get; set; }

        public int? EngineCC { get; set; }

        [MaxLength(20)]
        public string LicensePlate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Ready";

        [Range(0, double.MaxValue)]
        public decimal? DailyPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? HourlyPrice { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Range(1, 50)]
        public int? SeatingCapacity { get; set; }
    }
}
