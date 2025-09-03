using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_API.Repositories;
using UserManagement_API.DTOs;
using UserManagement_API.Services;

namespace Users_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // login không cần token
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IJwtTokenService _jwt;

        public AuthController(IUserRepository repo, IJwtTokenService jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
        {
            // validate input
            if (string.IsNullOrWhiteSpace(dto.UsersName) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Login failed",
                    Detail = "Username and password are required",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var user = await _repo.GetByUsernameAsync(dto.UsersName, ct);

            // 1. User không tồn tại
            if (user == null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Login failed",
                    Detail = "User does not exist",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            // 2. User bị inactive
            if (!user.IsActive)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Login failed",
                    Detail = "User is inactive",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            // 3. Sai mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Login failed",
                    Detail = "Incorrect password",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            // 4. Roles từ flags
            var roles = new List<string>();
            if (user.IsWebAdmin) roles.Add("Admin");
            if (user.IsSupervisor) roles.Add("Supervisor");
            if (user.IsHotelOwner) roles.Add("HotelOwner");
            if (user.IsTourAgency) roles.Add("TourAgency");
            if (user.IsVehicleAgency) roles.Add("VehicleAgency");

            // 5. Tạo token
            var accessToken = _jwt.CreateAccessToken(user.UserID, user.UsersName, roles, out var expiresAt);

            return Ok(new LoginResponseDto
            {
                Message = "Login successful",
                AccessToken = accessToken,
                ExpiresAt = expiresAt
            });
        }
    }
}
