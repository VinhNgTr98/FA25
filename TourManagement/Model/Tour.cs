using System;

namespace TourManagement.Model
{
    public class Tour
    {
        public Guid TourID { get; set; }

        // Không dùng ForeignKey, chỉ lưu ID
        public Guid TourAgencyID { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Itinerary { get; set; } // nvarchar(max)

        public string? Description { get; set; } // nvarchar(500)

        public int Length { get; set; } // Duration in days

        public string Transportation { get; set; } // varchar(50)

        public string? Resident { get; set; } // nvarchar(50)

        public int TourCapacity { get; set; }

        public decimal Price { get; set; } // decimal(10,2)
    }
}
