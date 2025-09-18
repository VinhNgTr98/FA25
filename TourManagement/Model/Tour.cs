using System;
using System.ComponentModel.DataAnnotations;

namespace TourManagement.Model
{
    public class Tour
    {
        [Key]
        public Guid TourID { get; set; }

        // Chỉ lưu ID, không dùng ForeignKey navigation
        public Guid AgencyID { get; set; }

        public string TourName { get; set; } = string.Empty; // nvarchar(200)
        public string Slug { get; set; } = string.Empty;     // nvarchar(200)

        public string StartingPoint { get; set; } = string.Empty; // nvarchar(200)
        public string Destination { get; set; } = string.Empty;   // nvarchar(200)

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int DurationDays { get; set; } // Duration in days

        public string? Description { get; set; } // nvarchar(500)

        public string Transportation { get; set; } = string.Empty; // varchar(50)

        public int MaxCapacity { get; set; }

        public decimal BasePrice { get; set; } // decimal(10,2)
        public string Currency { get; set; } = "VND"; // varchar(10)

        public string? Policies { get; set; } // nvarchar(max) (JSON string nếu cần)

        public string[] Languages { get; set; } = Array.Empty<string>();
        public string[] Tags { get; set; } = Array.Empty<string>();

        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
