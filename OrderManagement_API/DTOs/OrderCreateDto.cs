using System.ComponentModel.DataAnnotations;

namespace OrderManagement_API.DTOs
{
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "TotalAmount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be positive")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("Pending|Confirmed|Cancelled", ErrorMessage = "Status must be Pending, Confirmed or Cancelled")]
        public string Status { get; set; }

        public decimal? TaxAmount { get; set; }
    }
}
