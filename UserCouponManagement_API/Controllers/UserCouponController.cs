using Microsoft.AspNetCore.Mvc;
using UserCouponManagement_API.DTOs;
using UserCouponManagement_API.Services.Interfaces;

namespace UserCouponManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCouponController : ControllerBase
    {
        private readonly IUserCouponService _service;

        public UserCouponController(IUserCouponService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCouponReadDTO>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserCouponReadDTO>> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<UserCouponReadDTO>> Create(UserCouponCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.UserCouponId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserCouponUpdateDTO dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
