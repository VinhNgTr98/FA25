using System.ComponentModel.DataAnnotations;

namespace WishListManagement_API.DTOs
{
    public class CreateWishlistDto
    {
        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        public WishlistTargetType TargetType { get; set; }

        [Required]
        public Guid TargetId { get; set; }
    }
}
