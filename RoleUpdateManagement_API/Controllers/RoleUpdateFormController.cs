using Microsoft.AspNetCore.Mvc;
using RoleUpdateManagement_API.DTOs;
using RoleUpdateManagement_API.Services.Interfaces;

namespace RoleUpdateManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleUpdateFormController : ControllerBase
    {
        private readonly IRoleUpdateFormService _service;

        public RoleUpdateFormController(IRoleUpdateFormService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleUpdateFormReadDTO>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleUpdateFormReadDTO>> GetById(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<RoleUpdateFormReadDTO>> Create(RoleUpdateFormCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.FormId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, RoleUpdateFormUpdateDTO dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
