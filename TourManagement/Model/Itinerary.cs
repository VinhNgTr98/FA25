using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourManagement.Model
{
    public class Itinerary
    {
        [Key]
        public Guid ItineraryId { get; set; }

        public int? ItineraryOrder { get; set; }

        // Foreign Key → Tour
        public Guid TourID { get; set; }

        [ForeignKey(nameof(TourID))]
        public Tour? Tour { get; set; }

        public string? ItineraryTitles { get; set; } // nvarchar(max) (JSON string nếu cần)
        public string? ItineraryDetails { get; set; } // nvarchar(max) (JSON string nếu cần)

    }
}
