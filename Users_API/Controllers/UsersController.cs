using Microsoft.AspNetCore.Mvc;
using Users_API.Services;
using User_API.DTOs;
using UserManagement_API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Users_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] 
    public class UsersController : ControllerBase
    {
        private readonly IUserService _svc;
        public UsersController(IUserService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll(CancellationToken ct) =>
            Ok(await _svc.GetAllAsync(ct));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id, CancellationToken ct)
        {
            var u = await _svc.GetByIdAsync(id, ct);
            return u is null ? NotFound() : Ok(u);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserReadDto>> GetByUsername(string username, CancellationToken ct)
        {
            var u = await _svc.GetByUsernameAsync(username, ct);
            return u is null ? NotFound() : Ok(u);
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Create([FromBody] UserCreateDto dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.UserID }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto, CancellationToken ct)
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

        [HttpPost("{id:int}/otp")]
        public async Task<ActionResult<UserReadDto>> GenerateOtp(int id, CancellationToken ct)
        {
            var updated = await _svc.GenerateOtpAsync(id, ct);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpPost("{id:int}/verify")]
        public async Task<IActionResult> VerifyOtp(int id, [FromQuery] string code, CancellationToken ct)
        {
            var ok = await _svc.VerifyOtpAsync(id, code, ct);
            return ok ? Ok(new { message = "User verified" })
                      : BadRequest(new { message = "OTP invalid or expired" });
        }



    }
}
