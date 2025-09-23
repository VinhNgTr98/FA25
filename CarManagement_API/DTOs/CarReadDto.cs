using System.ComponentModel.DataAnnotations;

namespace CarManagement_API.DTOs
{
    public class CarReadDto
    {


        [Required]
        public Guid VehicleAgencyId { get; set; }
        public Guid CarId { get; set; }

        public string CarName { get; set; }

        public string CarBrand { get; set; }

        public string CarType { get; set; }

        public string Gear { get; set; }

        public string Engine { get; set; }

        public string LicensePlate { get; set; }

        public string Status { get; set; } = "Ready";

        public decimal? DailyPrice { get; set; }

        public decimal? HourlyPrice { get; set; }

        public string Description { get; set; }

        public int? SeatingCapacity { get; set; }
    }
}
