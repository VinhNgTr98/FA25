using CouponManagement_API.DTOs;
using CouponManagement_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CouponManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _service;

        public CouponController(ICouponService service)
        {
            _service = service;
        }

        // GET: api/coupon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponReadDTO>>> GetCoupons()
        {
            var coupons = await _service.GetAllCouponsAsync();
            return Ok(coupons);
        }

        // GET: api/coupon/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CouponReadDTO>> GetCoupon(int id)
        {
            var coupon = await _service.GetCouponByIdAsync(id);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        // POST: api/coupon
        [HttpPost]
        public async Task<ActionResult<CouponReadDTO>> CreateCoupon([FromBody] CouponCreateDTO dto)
        {
            var createdCoupon = await _service.CreateCouponAsync(dto);
            return CreatedAtAction(nameof(GetCoupon), new { id = createdCoupon.CouponId }, createdCoupon);
        }

        // PUT: api/coupon/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, [FromBody] CouponUpdateDTO dto)
        {
            var success = await _service.UpdateCouponAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/coupon/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var success = await _service.DeleteCouponAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
