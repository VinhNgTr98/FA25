using AutoMapper;
using FeedbackManagement_API.DTOs;
using FeedbackManagement_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;
        private readonly IMapper _mapper;

        public FeedbackController(IFeedbackService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/feedback?linkedType=room&linkedId={GUID}&page=1&pageSize=10&sort=recent|top
        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery] string linkedType,
            [FromQuery] Guid linkedId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sort = "recent",
            CancellationToken ct = default)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("page and pageSize must be > 0.");

            var (items, total) = await _service.GetPagedAsync(linkedType, linkedId, page, pageSize, sort, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(new
            {
                items = data,
                page,
                pageSize,
                totalItems = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }

        // GET /api/feedback/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FeedbackReadDTO>> GetById([FromRoute] int id, CancellationToken ct = default)
        {
            var entity = await _service.GetAsync(id, ct);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<FeedbackReadDTO>(entity));
        }

        // POST /api/feedback
        [HttpPost]
        public async Task<ActionResult<FeedbackReadDTO>> Create([FromBody] FeedbackCreateDTO dto, CancellationToken ct = default)
        {
            try
            {
                var entity = await _service.CreateAsync(dto, ct);
                var read = _mapper.Map<FeedbackReadDTO>(entity);
                return CreatedAtAction(nameof(GetById), new { id = read.FeedbackId }, read);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                // ReplyId không tồn tại, linked mismatch, thiếu rating...
                return BadRequest(ex.Message);
            }
        }

        // POST /api/feedback/{id}/reply
        [HttpPost("{id:int}/reply")]
        public async Task<ActionResult<FeedbackReadDTO>> Reply([FromRoute] int id, [FromBody] FeedbackReplyDTO dto, CancellationToken ct = default)
        {
            try
            {
                var entity = await _service.CreateReplyAsync(id, dto, ct);
                var read = _mapper.Map<FeedbackReadDTO>(entity);
                return CreatedAtAction(nameof(GetById), new { id = read.FeedbackId }, read);
            }
            catch (ArgumentException ex) when (ex.ParamName == "parentId")
            {
                // Parent feedback does not exist
                return NotFound(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/feedback/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<FeedbackReadDTO>> Update([FromRoute] int id, [FromBody] FeedbackUpdateDTO dto, CancellationToken ct = default)
        {
            try
            {
                var entity = await _service.UpdateAsync(id, dto, ct);
                if (entity is null) return NotFound();
                return Ok(_mapper.Map<FeedbackReadDTO>(entity));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/feedback/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct = default)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        // GET /api/feedback/summary?linkedType=room&linkedId={GUID}
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string linkedType, [FromQuery] Guid linkedId, CancellationToken ct = default)
        {
            var (avg, dist) = await _service.GetSummaryAsync(linkedType, linkedId, ct);
            return Ok(new { average = avg, distribution = dist });
        }
    }
}