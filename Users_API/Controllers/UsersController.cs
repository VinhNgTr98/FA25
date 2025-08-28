using Microsoft.AspNetCore.Mvc;
using User_API.DTOs;
using UserManagement_API.DTOs;
using Users_API.Services;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _svc;
    public UsersController(IUserService svc) => _svc = svc;

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserReadDto>> Get(int id, CancellationToken ct)
    {
        var u = await _svc.GetAsync(id, ct);
        return u == null ? NotFound() : Ok(u);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserReadDto>>> GetAll(CancellationToken ct)
    {
        var list = await _svc.GetAllAsync(ct);
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<UserReadDto>> Post([FromBody] UserCreateDto dto, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(Get), new { id = created.UserID }, created);
    }

    [HttpPut]
    public async Task<ActionResult<UserReadDto>> Put([FromBody] UserUpdateDto dto, CancellationToken ct)
    {
        var updated = await _svc.UpdateAsync(dto, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    // tăng cảnh báo
    [HttpPatch("{id:int}/warnings")]
    public async Task<ActionResult<UserReadDto>> IncreaseWarning(int id, [FromQuery] int by = 1, CancellationToken ct = default)
    {
        var updated = await _svc.IncreaseWarningAsync(id, by, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserReadDto>> GetByUsername(string username, CancellationToken ct)
    {
        var u = await _svc.GetByUsernameAsync(username, ct);
        return u == null ? NotFound() : Ok(u);
    }
}
