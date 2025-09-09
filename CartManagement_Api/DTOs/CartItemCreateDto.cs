using CartManagement_Api.Models;
using System.ComponentModel.DataAnnotations;

namespace CartManagement_Api.DTOs
{
    public class CartItemCreateDto
    {
        [Required] public CartItemType ItemType { get; set; }
        [Required] public Guid ItemID { get; set; }
        [Range(1, int.MaxValue)] public int Quantity { get; set; } = 1;
        public DateTime? StartDate { get; set; }  // For Room/Vehicle
        public DateTime? EndDate { get; set; }
    }
}
