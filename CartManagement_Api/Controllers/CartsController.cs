using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CartManagement_Api.Data;
using CartManagement_Api.Models;
using CartManagement_Api.DTOs;
using CartManagement_Api.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CartManagement_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize] 
    public class CartController : ControllerBase
    {
        private readonly ICartService _svc;
        public CartController(ICartService svc) => _svc = svc;

        private int? CurrentUserId =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        [HttpGet("me")]
        [AllowAnonymous]
        public async Task<ActionResult<CartReadDto>> GetMyCart(CancellationToken ct)
        {
            if (CurrentUserId is null) return Unauthorized();
            var cart = await _svc.GetOrCreateAsync(CurrentUserId.Value, ct);
            return Ok(cart);
        }

        [HttpPost("items")]
        [AllowAnonymous]
        public async Task<ActionResult<CartItemReadDto>> AddItem([FromBody] CartItemCreateDto dto, CancellationToken ct)
        {
            if (CurrentUserId is null) return Unauthorized();
            var item = await _svc.AddOrIncreaseAsync(CurrentUserId.Value, dto, ct);
            return CreatedAtAction(nameof(GetMyCart), new { }, item);
        }

        [HttpPut("items/{cartItemId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateItem(int cartItemId, [FromBody] CartItemUpdateDto dto, CancellationToken ct)
        {
            if (CurrentUserId is null) return Unauthorized();
            var ok = await _svc.UpdateItemAsync(CurrentUserId.Value, cartItemId, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("items/{cartItemId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveItem(int cartItemId, CancellationToken ct)
        {
            if (CurrentUserId is null) return Unauthorized();
            var ok = await _svc.RemoveItemAsync(CurrentUserId.Value, cartItemId, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("me")]
        [AllowAnonymous]
        public async Task<IActionResult> Clear(CancellationToken ct)
        {
            if (CurrentUserId is null) return Unauthorized();
            var ok = await _svc.ClearAsync(CurrentUserId.Value, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
