using ForumPostManagement_API.DTOs;
using ForumPostManagement_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPostManagement_API.Controllers
{
    [ApiController]
    [Route("api/forum-posts")]
    public class ForumPostsController : ControllerBase
    {
        private readonly IForumPostService _service;

        public ForumPostsController(IForumPostService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ForumPostReadDto>>> GetAll(CancellationToken ct)
        {
            var list = await _service.GetAllAsync(ct);
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ForumPostReadDto>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _service.GetByIdAsync(id, ct);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ForumPostReadDto>> Create([FromBody] ForumPostCreateDto dto, CancellationToken ct)
        {
            var created = await _service.CreateAsync(dto, ct: ct);
            return CreatedAtAction(nameof(GetById), new { id = created.ForumPostId }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ForumPostReadDto>> Update(Guid id, [FromBody] ForumPostUpdateDto dto, CancellationToken ct)
        {
            var updated = await _service.UpdateAsync(id, dto, regenerateSlugIfTitleChanged: true, ct: ct);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
        // GET by Type: /api/forum-posts/type/"Hotel" | "Tour" | "Vehicle"
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IReadOnlyList<ForumPostReadDto>>> GetByType(string type, CancellationToken ct)
        {
            var list = await _service.GetByTypeAsync(type, ct);
            return Ok(list);
        }

        // GET by Content: /api/forum-posts/search-content?keyword=xe%20máy
        [HttpGet("search-content")]
        public async Task<ActionResult<IReadOnlyList<ForumPostReadDto>>> GetByContent([FromQuery] string keyword, CancellationToken ct)
        {
            var list = await _service.GetByContentAsync(keyword, ct);
            return Ok(list);
        }

        [HttpPatch("{id:guid}/approval")]
        public async Task<ActionResult<ForumPostReadDto>> ChangeApprovalStatus(Guid id, [FromBody] ChangeApprovalStatusDto dto, CancellationToken ct)
        {
            try
            {
                var updated = await _service.ChangeApprovalStatusAsync(id, dto, ct);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}