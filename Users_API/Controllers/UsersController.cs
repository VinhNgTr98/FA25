using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using User_API.DTOs;
using UserManagement_API.DTOs;
using UserManagement_API.Services;

namespace Users_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _svc;
        public UsersController(IUserService svc) => _svc = svc;

        // ===== Helpers =====z
        //private int? CurrentUserId =>
        //    int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
        //private string? CurrentUserEmail => User.FindFirstValue(ClaimTypes.Email);
        private int? CurrentUserId =>
    int.TryParse(
        User.FindFirstValue(ClaimTypes.NameIdentifier) ??
        User.FindFirstValue(JwtRegisteredClaimNames.Sub),
        out var id) ? id : null;

        private string? CurrentUserEmail =>
            User.FindFirstValue(ClaimTypes.Email) ??
            User.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
        
        private bool IsAdmin => User.IsInRole("Admin");

        // ================= ADMIN ZONE =================

        /// <summary>Danh sách user – CHỈ Admin</summary>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll(CancellationToken ct)
            => Ok(await _svc.GetAllAsync(ct));

        // <summary>Admin tạo user nội bộ (KHÔNG tạo UsersInfo)</summary>
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> Create([FromBody] UserCreateDto dto, CancellationToken ct)
        {
            try
            {
                var (user, isResend) = await _svc.CreateWithInfoAsync(new UserWithInfoCreateDto
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

                if (isResend)
                    return Accepted(new { message = "OTP sent to your email" });

                return CreatedAtAction(nameof(GetById), new { id = user.UserID }, user);
            }
            catch (InvalidOperationException ex) when (ex.Message == "EMAIL_EXISTS")
            {
                return Conflict(new { message = "Email đã được đăng ký và đã xác thực. Vui lòng dùng email khác hoặc đăng nhập." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "OTP_RATE_LIMIT")
            {
                return StatusCode(StatusCodes.Status429TooManyRequests,
                    new { message = "Bạn vừa yêu cầu OTP quá nhanh hoặc vượt quá giới hạn trong ngày. Vui lòng thử lại sau." });
            }
        }


        /// <summary>Admin cập nhật bất kỳ user nào</summary>
        [HttpPut("{id:int}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUpdate(int id, [FromBody] UserUpdateDto dto, CancellationToken ct)
        {
            var ok = await _svc.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>Admin xoá user</summary>
        [HttpDelete("{id:int}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }

        // ================= USER / OWNER ZONE =================

        /// <summary>Lấy chi tiết user – Admin hoặc chính chủ</summary>
        [HttpGet("{id:int}")]
        // [Authorize]
        public async Task<ActionResult<UserReadDto>> GetById(int id, CancellationToken ct)
        {
           // if (!IsAdmin && CurrentUserId != id) return Forbid();
            var u = await _svc.GetByIdAsync(id, ct);
            return u is null ? NotFound() : Ok(u);
        }

        /// <summary>Lấy chi tiết theo username – Admin, hoặc chính chủ (username khớp)</summary>
        // [Authorize]
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserReadDto>> GetByEmail(string email, CancellationToken ct)
        {

          //  if (!IsAdmin && !string.Equals(CurrentUserEmail, email, StringComparison.OrdinalIgnoreCase))
          //      return Forbid();


            var u = await _svc.GetByEmailAsync(email, ct);
            return u is null ? NotFound() : Ok(u);
        }

        /// <summary>Người dùng xem thông tin của chính mình</summary>
        [HttpGet("me")]
        // [Authorize]
        public async Task<ActionResult<UserReadDto>> GetMe(CancellationToken ct)
        {
          //  if (CurrentUserId is null) return Unauthorized();

            var u = await _svc.GetByIdAsync(CurrentUserId.Value, ct);
            return u is null ? NotFound() : Ok(u);
        }

        /// <summary>Người dùng tự cập nhật profile của mình</summary>
        [HttpPut("me")]
        // [Authorize]
        public async Task<IActionResult> UpdateMe([FromBody] UserUpdateDto dto, CancellationToken ct)
        {
         //   if (CurrentUserId is null) return Unauthorized();

            var ok = await _svc.UpdateAsync(CurrentUserId.Value, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>Trả về roles hiện có trong JWT để FE hiển thị/disable UI.</summary>
        [HttpGet("me/roles")]
        //[Authorize]
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
                var (user, isResend) = await _svc.CreateWithInfoAsync(dto, ct);

                if (isResend)
                    return Accepted(new { message = "OTP sent to your email" });

                return CreatedAtAction(nameof(GetById), new { id = user.UserID }, user);
            }
            catch (InvalidOperationException ex) when (ex.Message == "EMAIL_EXISTS")
            {
                return Conflict(new { message = "Email đã được đăng ký và đã xác thực. Vui lòng dùng email khác hoặc đăng nhập." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "OTP_RATE_LIMIT")
            {
                return StatusCode(StatusCodes.Status429TooManyRequests,
                    new { message = "Bạn vừa yêu cầu OTP quá nhanh hoặc vượt quá giới hạn trong ngày. Vui lòng thử lại sau." });
            }
        }
        [HttpPost("{id:int}/otp")]
        [AllowAnonymous]
        public async Task<ActionResult> GenerateOtp(int id, CancellationToken ct)
        {
            try
            {
                var updated = await _svc.GenerateOtpAsync(id, ct);
                if (updated == null) return NotFound();

                // 202 Accepted cho action “send email”, không trả OTP
                return Accepted(new { message = "OTP sent to your email" });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("rate", StringComparison.OrdinalIgnoreCase))
            {
                return StatusCode(StatusCodes.Status429TooManyRequests,
                    new { message = "Too many OTP requests. Please wait before trying again." });
            }
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


        [HttpPost("{id:int}/change-password")]
        [AllowAnonymous] 
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordConfirmDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // Kiểm tra nhập lại mật khẩu mới khớp
            if (!string.Equals(dto.NewPassword, dto.ConfirmNewPassword, StringComparison.Ordinal))
                return BadRequest(new { message = "New password and confirmation do not match." });

            var ok = await _svc.ConfirmChangePasswordAsync(
                id,
                dto.OldPassword,
                dto.NewPassword,
                ct);

            return ok
                ? Ok(new { message = "Password changed" })
                : BadRequest(new { message = "Invalid old password or password policy not met." });
        }

        [HttpPatch("roles")]
        public async Task<IActionResult> UpdateSingleRole([FromBody] UpdateUserRoleDto dto, CancellationToken ct)
        {
            try
            {
                var ok = await _svc.UpdateSingleRoleAsync(dto, ct);
                if (!ok) return NotFound(new { message = "User không tồn tại." });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}