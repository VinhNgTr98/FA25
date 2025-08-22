using Microsoft.AspNetCore.Mvc;
using UsersInfoManagement_API.Dtos.UsersInfo;
using UsersInfoManagement_API.Services;

namespace UsersInfoManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/usersinfo
    public class UsersInfoController : ControllerBase
    {
        private readonly IUsersInfoService _service;
        public UsersInfoController(IUsersInfoService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersInfoReadDto>>> GetAll(CancellationToken ct)
            => Ok(await _service.GetAllAsync(ct));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsersInfoReadDto>> GetById(int id, CancellationToken ct)
        {
            var dto = await _service.GetByIdAsync(id, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        // tiện FE: lấy theo UsersID
        [HttpGet("~/api/users/{usersId:int}/info")]
        public async Task<ActionResult<UsersInfoReadDto>> GetByUserId(int usersId, CancellationToken ct)
        {
            var dto = await _service.GetByUserIdAsync(usersId, ct);
            return dto is null ? NotFound() : Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult<UsersInfoReadDto>> Create([FromBody] UsersInfoCreateDto dto, CancellationToken ct)
        {
            try
            {
                var created = await _service.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id = created.UsersInfoID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsersInfoUpdateDto dto, CancellationToken ct)
        {
            var ok = await _service.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
