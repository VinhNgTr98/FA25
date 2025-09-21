using CartManagement_Api.DTOs;
using CartManagement_Api.Services;
using CartManagement_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartManagement_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;

        public CartController(ICartService cartService, ICartItemService cartItemService)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
        }

        // ===== Cart =====
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartReadDto>>> GetAllCarts()
        {
            return Ok(await _cartService.GetAllAsync());
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<CartReadDto>> GetCart(int cartId)
        {
            var cart = await _cartService.GetByIdAsync(cartId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<CartReadDto>> CreateCart([FromBody] CartCreateDto dto)
        {
            var created = await _cartService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetCart), new { cartId = created.CartID }, created);
        }

        [HttpPut("{cartId}")]
        public async Task<ActionResult<CartReadDto>> UpdateCart(int cartId, [FromBody] CartUpdateDto dto)
        {
            var updated = await _cartService.UpdateAsync(cartId, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            var success = await _cartService.DeleteAsync(cartId);
            if (!success) return NotFound();
            return NoContent();
        }

        // ===== CartItem xử lý tại Controller =====
        [HttpPost("{userId}/items")]
        public async Task<ActionResult<CartItemReadDto>> AddItemToCart(int userId, [FromBody] CartItemCreateDto dto)
        {
            // 1. Kiểm tra user đã có cart chưa
            var allCarts = await _cartService.GetAllAsync();
            var cart = allCarts.FirstOrDefault(c => c.UserID == userId);

            if (cart == null)
            {
                // tạo cart mới
                var newCart = await _cartService.CreateAsync(new CartCreateDto { UserID = userId });
                cart = newCart;
            }

            // 2. Lấy toàn bộ item trong cart
            var allItems = await _cartItemService.GetAllAsync();
            var itemsOfCart = allItems.Where(i => i.CartID == cart.CartID);

            // 3. Kiểm tra trùng Item
            var existing = itemsOfCart.FirstOrDefault(i =>
                i.ItemType == dto.ItemType &&
                i.ItemID == dto.ItemID &&
                i.StartDate == dto.StartDate &&
                i.EndDate == dto.EndDate
            );

            if (existing != null)
            {
                // update số lượng
                var updateDto = new CartItemUpdateDto
                {
                    ItemType = existing.ItemType,
                    ItemID = existing.ItemID,
                    Quantity = existing.Quantity + dto.Quantity,
                    StartDate = existing.StartDate,
                    EndDate = existing.EndDate
                };

                var updated = await _cartItemService.UpdateAsync(existing.CartItemID, updateDto);
                return Ok(updated);
            }

            // 4. Nếu chưa có hoặc khác ngày thì thêm mới
            dto.CartID = cart.CartID;
            var created = await _cartItemService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{cartId}/items/{itemId}")]
        public async Task<ActionResult<CartItemReadDto>> UpdateCartItem(int cartId, int itemId, [FromBody] CartItemUpdateDto dto)
        {
            var updated = await _cartItemService.UpdateAsync(itemId, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{cartId}/items/{itemId}")]
        public async Task<IActionResult> DeleteCartItem(int cartId, int itemId)
        {
            var success = await _cartItemService.DeleteAsync(itemId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}