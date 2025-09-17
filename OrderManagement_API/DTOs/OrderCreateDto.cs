using System.ComponentModel.DataAnnotations;

namespace OrderManagement_API.DTOs
{
    public class OrderCreateDto
    {
        [Required] public int UserID { get; set; }
        [Range(0, double.MaxValue)] public decimal? TaxAmount { get; set; }
        public int? CouponId { get; set; }
        [StringLength(500)] public string? OrderNote { get; set; }
    }
}