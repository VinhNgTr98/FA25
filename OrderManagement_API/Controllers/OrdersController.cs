// Controllers/OrdersController.cs
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAll(CancellationToken ct)
            => Ok(await _svc.GetAllAsync(ct));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderReadDto>> GetById(int id, CancellationToken ct)
        {
            var o = await _svc.GetByIdAsync(id, ct);
            return o == null ? NotFound() : Ok(o);
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> Create([FromBody] OrderCreateDto dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.OrderID }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDto dto, CancellationToken ct)
        {
            var ok = await _svc.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
