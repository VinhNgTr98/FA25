using System.ComponentModel.DataAnnotations.Schema;
using TourManagement.Model;

namespace TourManagement.DTOs
{
    public class ItineraryUpdateDTO
    {
        public Guid ItineraryId { get; set; }

        public int? ItineraryOrder { get; set; }


        public string? ItineraryTitles { get; set; } // nvarchar(max) (JSON string nếu cần)
        public string? ItineraryDetails { get; set; }
    }
}
