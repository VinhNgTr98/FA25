using System.Security.Claims;
using CartManagement_Api.DTOs;
using CartManagement_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartManagement_Api.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize] // Sau khi mock JWT
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service) => _service = service;

        private int GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(claim, out var id)) return id;
            if (Request.Query.TryGetValue("userId", out var q) && int.TryParse(q, out id)) return id;
            throw new ArgumentException("Cannot determine user id");
        }

        [HttpGet]
        public async Task<ActionResult<CartReadDto>> Get(CancellationToken ct)
        {
            var cart = await _service.GetOrCreateCartAsync(GetUserId(), ct);
            return Ok(cart);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<CartSummaryDto>> Summary(CancellationToken ct)
        {
            var sum = await _service.GetSummaryAsync(GetUserId(), ct);
            return Ok(sum);
        }

        [HttpGet("items/{id:int}")]
        public async Task<ActionResult<CartItemReadDto>> GetItem(int id, CancellationToken ct)
        {
            var cart = await _service.GetCartByUserAsync(GetUserId(), ct);
            if (cart == null) return NotFound();
            var item = cart.Items.FirstOrDefault(i => i.CartItemID == id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartReadDto>> AddItem([FromBody] CartItemCreateDto req, CancellationToken ct)
        {
            var userId = GetUserId();
            var before = await _service.GetCartByUserAsync(userId, ct);
            var after = await _service.AddItemAsync(userId, req, ct);

            if (IsNew(before, after, req))
                return CreatedAtAction(nameof(Get), new { }, after);
            return Ok(after);
        }

        private static bool IsNew(CartReadDto? before, CartReadDto after, CartItemCreateDto req)
        {
            if (before == null) return true;
            if (after.Items.Count > before.Items.Count) return true;
            var existedBefore = before.Items.Any(i =>
                i.ItemType.Equals(req.ItemType, StringComparison.OrdinalIgnoreCase) &&
                i.ItemID == req.ItemID &&
                i.StartDate == req.StartDate &&
                i.EndDate == req.EndDate);
            var existsAfter = after.Items.Any(i =>
                i.ItemType.Equals(req.ItemType, StringComparison.OrdinalIgnoreCase) &&
                i.ItemID == req.ItemID &&
                i.StartDate == req.StartDate &&
                i.EndDate == req.EndDate);
            return !existedBefore && existsAfter;
        }

        [HttpPatch("items/{id:int}")]
        public async Task<ActionResult<CartReadDto>> UpdateItem(int id, [FromBody] CartItemUpdateDto req, CancellationToken ct)
        {
            var cart = await _service.UpdateItemAsync(GetUserId(), id, req, ct);
            return Ok(cart);
        }

        [HttpDelete("items/{id:int}")]
        public async Task<IActionResult> RemoveItem(int id, [FromQuery] string rowVersion, CancellationToken ct)
        {
            var ok = await _service.RemoveItemAsync(GetUserId(), id, rowVersion, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear(CancellationToken ct)
        {
            await _service.ClearCartAsync(GetUserId(), ct);
            return NoContent();
        }
    }
}