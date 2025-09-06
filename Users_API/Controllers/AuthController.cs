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
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Login failed",
                    Detail = "Email and password are required",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var user = await _repo.GetByEmailAsync(dto.Email, ct);

            if (user == null)
                return Unauthorized(new ProblemDetails { Title = "Login failed", Detail = "User does not exist", Status = 401 });

            if (!user.IsActive)
                return Unauthorized(new ProblemDetails { Title = "Login failed", Detail = "User is inactive", Status = 401 });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new ProblemDetails { Title = "Login failed", Detail = "Incorrect password", Status = 401 });

            var roles = new List<string>();
            if (user.IsWebAdmin) roles.Add("Admin");
            if (user.IsSupervisor) roles.Add("Supervisor");
            if (user.IsHotelOwner) roles.Add("HotelOwner");
            if (user.IsTourAgency) roles.Add("TourAgency");
            if (user.IsVehicleAgency) roles.Add("VehicleAgency");

            var accessToken = _jwt.CreateAccessToken(user.UserID, user.Email, roles, out var expiresAt);

            return Ok(new LoginResponseDto
            {
                Message = "Login successful",
                AccessToken = accessToken,
                ExpiresAt = expiresAt
            });
        }

    }
}
