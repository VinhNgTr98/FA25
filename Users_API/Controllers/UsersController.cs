using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Users_API.Services;
using User_API.DTOs;
using UserManagement_API.DTOs;

namespace Users_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _svc;
        public UsersController(IUserService svc) => _svc = svc;

        // ===== Helpers =====
        private int? CurrentUserId =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        private string? CurrentUserEmail => User.FindFirstValue(ClaimTypes.Email);
        private bool IsAdmin => User.IsInRole("Admin");

        // ================= ADMIN ZONE =================

        /// <summary>Danh sách user – CHỈ Admin</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll(CancellationToken ct)
            => Ok(await _svc.GetAllAsync(ct));

        /// <summary>Admin tạo user nội bộ (KHÔNG tạo UsersInfo)</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> Create([FromBody] UserCreateDto dto, CancellationToken ct)
        {
            var created = await _svc.CreateWithInfoAsync(new UserWithInfoCreateDto
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Password = dto.Password,
                IsHotelOwner = dto.IsHotelOwner,
                IsTourAgency = dto.IsTourAgency,
                IsVehicleAgency = dto.IsVehicleAgency,
                IsWebAdmin = dto.IsWebAdmin,
                IsModerator = dto.IsModerator
            }, ct);

            return CreatedAtAction(nameof(GetById), new { id = created.UserID }, created);
        }

        /// <summary>Admin cập nhật bất kỳ user nào</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> AdminUpdate(int id, [FromBody] UserUpdateDto dto, CancellationToken ct)
        {
            var ok = await _svc.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>Admin xoá user</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }

        // ================= USER / OWNER ZONE =================

        /// <summary>Lấy chi tiết user – Admin hoặc chính chủ</summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id, CancellationToken ct)
        {
            if (!IsAdmin && CurrentUserId != id) return Forbid();
            var u = await _svc.GetByIdAsync(id, ct);
            return u is null ? NotFound() : Ok(u);
        }

        /// <summary>Lấy chi tiết theo username – Admin, hoặc chính chủ (username khớp)</summary>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserReadDto>> GetByEmail(string email, CancellationToken ct)
        {
            if (!IsAdmin && !string.Equals(CurrentUserEmail, email, StringComparison.OrdinalIgnoreCase))
                return Forbid();

            var u = await _svc.GetByEmailAsync(email, ct);
            return u is null ? NotFound() : Ok(u);
        }

        /// <summary>Người dùng xem thông tin của chính mình</summary>
        [HttpGet("me")]
        public async Task<ActionResult<UserReadDto>> GetMe(CancellationToken ct)
        {
            if (CurrentUserId is null) return Unauthorized();
            var u = await _svc.GetByIdAsync(CurrentUserId.Value, ct);
            return u is null ? NotFound() : Ok(u);
        }

        /// <summary>Người dùng tự cập nhật profile của mình</summary>
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] UserUpdateDto dto, CancellationToken ct)
        {
            if (CurrentUserId is null) return Unauthorized();
            var ok = await _svc.UpdateAsync(CurrentUserId.Value, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>Trả về roles hiện có trong JWT để FE hiển thị/disable UI.</summary>
        [HttpGet("me/roles")]
        [Authorize]
        public ActionResult<object> GetMyRoles()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .Distinct()
                .ToArray();
            return Ok(new { roles });
        }

        // ================= PUBLIC REGISTER (NO TOKEN) =================

        /// <summary>
        /// Public register: tạo User + UsersInfo trong 1 lần.
        /// </summary>
        [HttpPost("public/register-with-info")]
        [AllowAnonymous]
        public async Task<ActionResult<UserReadDto>> PublicRegisterWithInfo([FromBody] UserWithInfoCreateDto dto, CancellationToken ct)
        {
            try
            {
                var created = await _svc.CreateWithInfoAsync(dto, ct);
                var withOtp = await _svc.GenerateOtpAsync(created.UserID, ct);
                return CreatedAtAction(nameof(GetById), new { id = withOtp.UserID }, withOtp);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>Xin OTP cho user chưa active (ví dụ sau khi đăng ký) - PUBLIC ACCESS</summary>
        [HttpPost("{id:int}/otp")]
        [AllowAnonymous] // Đảm bảo public access
        public async Task<ActionResult<UserReadDto>> GenerateOtp(int id, CancellationToken ct)
        {
            var updated = await _svc.GenerateOtpAsync(id, ct);
            return updated == null ? NotFound() : Ok(updated);
        }

        /// <summary>Verify OTP (public – thường dùng ngay sau khi đăng ký)</summary>
        [HttpPost("{id:int}/verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp(int id, [FromQuery] string code, CancellationToken ct)
        {
            var ok = await _svc.VerifyOtpAsync(id, code, ct);
            return ok ? Ok(new { message = "User verified" })
                      : BadRequest(new { message = "OTP invalid or expired" });
        }
    }
}