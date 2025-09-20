using System.ComponentModel.DataAnnotations;

namespace BookingManagement_API.DTOs
{
    public class BookingCreateDto
    {
        [Required] public int UserID { get; set; }
        [Range(0, double.MaxValue)] public decimal? TaxAmount { get; set; }
        public int? CouponId { get; set; }
        [StringLength(500)] public string? BookingNote { get; set; }
    }
}