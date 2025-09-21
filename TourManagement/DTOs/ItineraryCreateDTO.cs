using System.ComponentModel.DataAnnotations.Schema;
using TourManagement.Model;

namespace TourManagement.DTOs
{
    public class ItineraryCreateDTO
    {
        //public Guid ItineraryId { get; set; }

        public int? ItineraryOrder { get; set; }

        // Foreign Key → Tour
        public Guid TourID { get; set; }

        public string? ItineraryTitles { get; set; } // nvarchar(max) (JSON string nếu cần)
        public string? ItineraryDetails { get; set; }
    }
}
