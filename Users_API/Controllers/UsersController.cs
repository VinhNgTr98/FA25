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
            return ok ? NoContent() : NotFound("User is locked due to too many warnings");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }

        // ⬇️ NEW: lấy roles theo userId
        [HttpGet("{id:int}/roles")]
        [Authorize] // nếu muốn public thì bỏ attribute này
        public async Task<ActionResult<IEnumerable<string>>> GetRoles(int id, CancellationToken ct)
        {
            var roles = await _svc.GetRolesAsync(id, ct);
            if (roles == null) return NotFound(new { message = "User not found" });
            return Ok(roles);
        }

        // ⬇️ NEW: lấy roles của chính user đang đăng nhập (tiện cho FE)
        [HttpGet("me/roles")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetMyRoles(CancellationToken ct)
        {
            var sub = User.FindFirst("sub")?.Value
                      ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub) || !int.TryParse(sub, out var userId))
                return Unauthorized(new { message = "Invalid token" });

            var roles = await _svc.GetRolesAsync(userId, ct);
            if (roles == null) return NotFound(new { message = "User not found" });
            return Ok(roles);
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
