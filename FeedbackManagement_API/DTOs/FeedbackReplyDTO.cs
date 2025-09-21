using System.ComponentModel.DataAnnotations;

namespace FeedbackManagement_API.DTOs
{
    public class FeedbackReplyDTO
    {
        [Required]
        public int UserID { get; set; } 

        [Required, StringLength(500)]
        public string? Comment { get; set; }
    }
}