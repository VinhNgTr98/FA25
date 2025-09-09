using System.ComponentModel.DataAnnotations;

namespace CartManagement_Api.DTOs
{
    public class CartItemUpdateDto
    {
        [Range(1, int.MaxValue)] public int Quantity { get; set; } = 1;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
