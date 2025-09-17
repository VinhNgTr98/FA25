using Microsoft.AspNetCore.Mvc;
using OrderManagement_API.DTOs;
using OrderManagement_API.Services;

namespace OrderManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _svc;
        public OrdersController(IOrderService svc) => _svc = svc;

        // GET /api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAll(CancellationToken ct)
            => Ok(await _svc.GetAllAsync(ct));

        // GET /api/orders/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderReadDto>> GetById(int id, CancellationToken ct)
        {
            var o = await _svc.GetByIdAsync(id, ct);
            return o == null ? NotFound() : Ok(o);
        }

        // GET /api/orders/by-user/{userId}
        [HttpGet("by-user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetByUserId(int userId, CancellationToken ct)
            => Ok(await _svc.GetByUserIdAsync(userId, ct));

        // GET /api/orders/{orderId}/items
        [HttpGet("{orderId:int}/items")]
        public async Task<ActionResult<IEnumerable<OrderItemReadDto>>> GetItemsByOrderId(int orderId, CancellationToken ct)
            => Ok(await _svc.GetItemsByOrderIdAsync(orderId, ct));

        // POST /api/orders
        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> Create([FromBody] OrderCreateDto dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.OrderID }, created);
        }

        // POST /api/orders/{orderId}/items
        [HttpPost("{orderId:int}/items")]
        public async Task<ActionResult<OrderItemReadDto>> AddItem(int orderId, [FromBody] OrderItemCreateDto dto, CancellationToken ct)
        {
            var created = await _svc.AddItemAsync(orderId, dto, ct);
            if (created == null) return NotFound("Order not found.");
            // Trả về 201 và Location trỏ đến danh sách items của order
            return CreatedAtAction(nameof(GetItemsByOrderId), new { orderId }, created);
        }

        // PUT /api/orders/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDto dto, CancellationToken ct)
        {
            var ok = await _svc.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        // DELETE /api/orders/{id}
        // Chặn xóa nếu trạng thái là Accepted
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var existing = await _svc.GetByIdAsync(id, ct);
            if (existing == null) return NotFound();

            if (string.Equals(existing.Status, "Accepted", StringComparison.OrdinalIgnoreCase))
                return Conflict("Order has been accepted and cannot be deleted.");

            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}