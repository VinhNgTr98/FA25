using System.ComponentModel.DataAnnotations;

namespace FeedbackManagement_API.DTOs
{
    public class FeedbackUpdateDTO
    {
        [Range(1, 5)]
        public int? Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}
