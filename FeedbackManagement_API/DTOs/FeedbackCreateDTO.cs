using System.ComponentModel.DataAnnotations;

namespace FeedbackManagement_API.DTOs
{
    public class FeedbackCreateDTO
    {
        [Required]
        public int UserID { get; set; } 

        [Required]
        public Guid LinkedId { get; set; }

        [Required, StringLength(30)]
        public string LinkedType { get; set; } = default!;

        [Range(1, 5)]
        public int? Rating { get; set; } // Cho phép null khi là reply (tuỳ policy)

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}