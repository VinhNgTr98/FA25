using System.Threading;
using System.Threading.Tasks;
using CartManagement_Api.DTOs;
using CartManagement_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartManagement_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        // GET /api/cart/{userId}
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<CartReadDto>> GetCart(int userId, CancellationToken ct)
        {
            var cart = await _service.GetCartByUserIdAsync(userId, ct);
            return Ok(cart);
        }

        // POST /api/cart/{userId}/items
        [HttpPost("{userId:int}/items")]
        public async Task<ActionResult<CartItemReadDto>> AddItem(
            int userId,
            [FromBody] CartItemCreateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var item = await _service.AddItemAsync(userId, dto, ct);

            // 201 Created
            return CreatedAtAction(nameof(GetCart), new { userId }, item);
        }

        // PUT /api/cart/items/{cartItemId}
        [HttpPut("items/{cartItemId:int}")]
        public async Task<ActionResult<CartItemReadDto>> UpdateItem(
            int cartItemId,
            [FromBody] CartItemUpdateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var updated = await _service.UpdateItemAsync(cartItemId, dto, ct);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE /api/cart/items/{cartItemId}
        [HttpDelete("items/{cartItemId:int}")]
        public async Task<IActionResult> RemoveItem(int cartItemId, CancellationToken ct)
        {
            var removed = await _service.RemoveItemAsync(cartItemId, ct);
            if (!removed) return NotFound();
            return NoContent();
        }

        // DELETE /api/cart/{userId}
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> ClearCart(int userId, CancellationToken ct)
        {
            var cleared = await _service.ClearCartAsync(userId, ct);
            if (!cleared) return NotFound();
            return NoContent();
        }
    }
}