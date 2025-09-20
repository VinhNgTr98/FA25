using System.ComponentModel.DataAnnotations;

namespace OrderManagement_API.DTOs
{
    public class BookingItemCreateDto
    {
        [Required, StringLength(32)] public string ItemType { get; set; } = default!;
        [Required] public Guid ItemID { get; set; }
        [Range(1, int.MaxValue)] public int Quantity { get; set; }
        [Range(0, double.MaxValue)] public decimal UnitPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}