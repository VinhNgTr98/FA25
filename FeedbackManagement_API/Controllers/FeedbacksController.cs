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

        // EXISTING: GetList by linkedType & linkedId (đã có phân trang)
        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery] string linkedType,
            [FromQuery] Guid linkedId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sort = "recent",
            CancellationToken ct = default)
        {
            if (!ValidatePage(ref page, ref pageSize, out var bad)) return bad!;
            var (items, total) = await _service.GetPagedAsync(linkedType, linkedId, page, pageSize, sort, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(Paged(data, page, pageSize, total));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FeedbackReadDTO>> GetById([FromRoute] int id, CancellationToken ct = default)
        {
            var entity = await _service.GetAsync(id, ct);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<FeedbackReadDTO>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<FeedbackReadDTO>> Create([FromBody] FeedbackCreateDTO dto, CancellationToken ct = default)
        {
            try
            {
                var entity = await _service.CreateAsync(dto, ct);
                var read = _mapper.Map<FeedbackReadDTO>(entity);
                return CreatedAtAction(nameof(GetById), new { id = read.FeedbackId }, read);
            }
            catch (ArgumentOutOfRangeException ex) { return BadRequest(ex.Message); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("{id:int}/reply")]
        public async Task<ActionResult<FeedbackReadDTO>> Reply([FromRoute] int id, [FromBody] FeedbackReplyDTO dto, CancellationToken ct = default)
        {
            try
            {
                var entity = await _service.CreateReplyAsync(id, dto, ct);
                var read = _mapper.Map<FeedbackReadDTO>(entity);
                return CreatedAtAction(nameof(GetById), new { id = read.FeedbackId }, read);
            }
            catch (ArgumentException ex) when (ex.ParamName == "parentId") { return NotFound(ex.Message); }
            catch (ArgumentOutOfRangeException ex) { return BadRequest(ex.Message); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<FeedbackReadDTO>> Update([FromRoute] int id, [FromBody] FeedbackUpdateDTO dto, CancellationToken ct = default)
        {
            try
            {
                var entity = await _service.UpdateAsync(id, dto, ct);
                if (entity is null) return NotFound();
                return Ok(_mapper.Map<FeedbackReadDTO>(entity));
            }
            catch (ArgumentOutOfRangeException ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct = default)
        {
            var ok = await _service.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string linkedType, [FromQuery] Guid linkedId, CancellationToken ct = default)
        {
            var (avg, dist) = await _service.GetSummaryAsync(linkedType, linkedId, ct);
            return Ok(new { average = avg, distribution = dist });
        }

        // NEW: generic gets (paged), pageSize giới hạn 1..10
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            if (!ValidatePage(ref page, ref pageSize, out var bad)) return bad!;
            var (items, total) = await _service.GetAllPagedAsync(page, pageSize, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(Paged(data, page, pageSize, total));
        }

        [HttpGet("byLinkedType/{linkedType}")]
        public async Task<IActionResult> GetByLinkedType([FromRoute] string linkedType, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            if (!ValidatePage(ref page, ref pageSize, out var bad)) return bad!;
            var (items, total) = await _service.GetByLinkedTypePagedAsync(linkedType, page, pageSize, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(Paged(data, page, pageSize, total));
        }

        [HttpGet("byLinkedId/{linkedId}")]
        public async Task<IActionResult> GetByLinkedId([FromRoute] Guid linkedId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            if (!ValidatePage(ref page, ref pageSize, out var bad)) return bad!;
            var (items, total) = await _service.GetByLinkedIdPagedAsync(linkedId, page, pageSize, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(Paged(data, page, pageSize, total));
        }

        [HttpGet("byUser/{userID:int}")]
        public async Task<IActionResult> GetByUser([FromRoute] int userID, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            if (!ValidatePage(ref page, ref pageSize, out var bad)) return bad!;
            var (items, total) = await _service.GetByUserIDPagedAsync(userID, page, pageSize, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(Paged(data, page, pageSize, total));
        }

        [HttpGet("byRating/{rating:int}")]
        public async Task<IActionResult> GetByRating([FromRoute] int rating, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            if (rating < 1 || rating > 5) return BadRequest("Rating must be between 1 and 5.");
            if (!ValidatePage(ref page, ref pageSize, out var bad)) return bad!;
            var (items, total) = await _service.GetByRatingPagedAsync(rating, page, pageSize, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(Paged(data, page, pageSize, total));
        }

        // Replies:
        // 1) preview: trả 1 bản ghi mới nhất
        [HttpGet("replies/{feedbackId:int}/preview")]
        public async Task<IActionResult> GetRepliesPreview([FromRoute] int feedbackId, CancellationToken ct = default)
        {
            var (items, total) = await _service.GetRepliesByFeedbackIdPagedAsync(feedbackId, 1, 1, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(new
            {
                items = data,
                totalItems = total,
                hasMore = total > 1
            });
        }

        // 2) paged: xem thêm (client điều chỉnh page/pageSize, mặc định 10, giới hạn 1..10)
        [HttpGet("replies/{feedbackId:int}")]
        public async Task<IActionResult> GetRepliesPaged([FromRoute] int feedbackId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            if (!ValidatePage(ref page, ref pageSize, out var bad)) return bad!;
            var (items, total) = await _service.GetRepliesByFeedbackIdPagedAsync(feedbackId, page, pageSize, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(Paged(data, page, pageSize, total));
        }

        // 3) all: trả hết (nếu bạn muốn đúng nghĩa “getall” khi bấm xem thêm)
        [HttpGet("replies/{feedbackId:int}/all")]
        public async Task<IActionResult> GetRepliesAll([FromRoute] int feedbackId, CancellationToken ct = default)
        {
            var items = await _service.GetRepliesByFeedbackIdAsync(feedbackId, ct);
            var data = items.Select(_mapper.Map<FeedbackReadDTO>);
            return Ok(new
            {
                items = data,
                totalItems = items.LongCount()
            });
        }

        private static bool ValidatePage(ref int page, ref int pageSize, out BadRequestObjectResult? bad)
        {
            bad = null;
            if (page < 1) page = 1;

            // Giới hạn pageSize trong [1..10] như yêu cầu
            if (pageSize < 1 || pageSize > 10)
            {
                bad = new BadRequestObjectResult("pageSize must be between 1 and 10.");
                return false;
            }
            return true;
        }

        private static object Paged(IEnumerable<FeedbackReadDTO> items, int page, int pageSize, long total)
        {
            return new
            {
                items,
                page,
                pageSize,
                totalItems = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
        }
    }
}