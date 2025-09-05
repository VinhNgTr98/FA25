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
    //[Authorize] 
    public class UsersController : ControllerBase
    {
        private readonly IUserService _svc;
        public UsersController(IUserService svc) => _svc = svc;

        // ===== Helpers =====
        private int? CurrentUserId =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        private string? CurrentUserName => User.Identity?.Name;
        private bool IsAdmin => User.IsInRole("Admin");

        // ================= ADMIN ZONE =================

        /// <summary>Danh sách user – CHỈ Admin</summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll(CancellationToken ct)
            => Ok(await _svc.GetAllAsync(ct));

        /// <summary>Admin tạo user nội bộ (staff/đối tác). 
        /// Nếu khách tự đăng ký, dùng endpoint /public/register (ở nơi khác nếu có).</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> Create([FromBody] UserCreateDto dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.UserID }, created);
        }

        /// <summary>Admin cập nhật bất kỳ user nào</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUpdate(int id, [FromBody] UserUpdateDto dto, CancellationToken ct)
        {
            var ok = await _svc.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>Admin xoá user</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
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
        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserReadDto>> GetByUsername(string username, CancellationToken ct)
        {
            if (!IsAdmin && !string.Equals(CurrentUserName, username, StringComparison.OrdinalIgnoreCase))
                return Forbid();

            var u = await _svc.GetByUsernameAsync(username, ct);
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

        /// <summary>Xin mã OTP – Admin hoặc chính chủ</summary>
        [HttpPost("{id:int}/otp")]
        public async Task<ActionResult<UserReadDto>> GenerateOtp(int id, CancellationToken ct)
        {
            if (!IsAdmin && CurrentUserId != id) return Forbid();
            var updated = await _svc.GenerateOtpAsync(id, ct);
            return updated == null ? NotFound() : Ok(updated);
        }

       

        // ================= FE INTROSPECTION =================

        /// <summary>Trả về roles hiện có trong JWT để FE hiển thị/disable UI.</summary>
        [HttpGet("me/roles")]
        public ActionResult<object> GetMyRoles()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .Distinct()
                .ToArray();
            return Ok(new { roles });
        }

        // ================= PUBLIC REGISTER (NO TOKEN, OTP-FIRST) =================

        /// Server ép mọi role=false, IsActive=false, sinh OTP và TRẢ OTP cho FE để FE gửi email.
        [HttpPost("public/register")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> PublicRegister([FromBody] UserCreateDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.UsersName) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "UsersName and Password are required" });
            }

            // Check trùng username
            var existedByName = await _svc.GetByUsernameAsync(dto.UsersName, ct);
            if (existedByName != null)
                return Conflict(new { message = "Username already exists" });

            // ép quyền mặc định = false
            dto.IsWebAdmin = dto.IsSupervisor = dto.IsHotelOwner = dto.IsTourAgency = dto.IsVehicleAgency = false;
            dto.IsActive = false;
            dto.otp_code = null;
            dto.otp_expires = null;

            // Tạo user
            var created = await _svc.CreateAsync(dto, ct);

            // Sinh OTP
            var withOtp = await _svc.GenerateOtpAsync(created.UserID, ct);

            // Trả OTP cho FE để FE tự gửi email 
            return CreatedAtAction(nameof(GetById), new { id = created.UserID }, new
            {
                userId = withOtp.UserID,
                usersName = withOtp.UsersName,
                otpCode = withOtp.otp_code,
                otpExpires = withOtp.otp_expires
            });
        }

        
        /// Resend OTP: không cần token. FE có thể gọi để xin lại OTP nếu khách chưa active.
        [HttpPost("public/otp/generate")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateOrResendOtp([FromQuery] string usersName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(usersName))
                return BadRequest(new { message = "usersName is required" });

            var user = await _svc.GetByUsernameAsync(usersName, ct);
            if (user is null) return NotFound(new { message = "User not found" });
            if (user.IsActive) return BadRequest(new { message = "User already active" });

            var withOtp = await _svc.GenerateOtpAsync(user.UserID, ct);

            return Ok(new
            {
                userId = withOtp.UserID,
                usersName = withOtp.UsersName,
                otpCode = withOtp.otp_code,
                otpExpires = withOtp.otp_expires
            });
        }

        [HttpPost("{id:int}/verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp(int id, [FromQuery] string code, CancellationToken ct)
        {
            var ok = await _svc.VerifyOtpAsync(id, code, ct);
            return ok ? Ok(new { message = "User verified" })
                      : BadRequest(new { message = "OTP invalid or expired" });
        }


        ///////////////////////
    }
}
