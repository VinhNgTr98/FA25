using System.ComponentModel.DataAnnotations;

namespace WishListManagement_API.DTOs
{
    public class UpdateWishlistDto
    {
        [Required]
        public WishlistTargetType TargetType { get; set; }

        [Required]
        public Guid TargetId { get; set; }
    }
}
