using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_API.DTOs;
using User_API.Repositories;
using UserManagement_API.DTOs;
using UserManagement_API.Services;

namespace Users_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IJwtTokenService _jwt;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository repo, IJwtTokenService jwt, IMapper mapper)
        {
            _repo = repo;
            _jwt = jwt;
            _mapper = mapper;
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
            if (user.IsModerator) roles.Add("Moderator");
            if (user.IsHotelOwner) roles.Add("HotelOwner");
            if (user.IsTourAgency) roles.Add("TourAgency");
            if (user.IsVehicleAgency) roles.Add("VehicleAgency");

            var accessToken = _jwt.CreateAccessToken(user.UserID, user.Email, roles, out var expiresAt);

            var userDto = _mapper.Map<UserReadDto>(user);

            return Ok(new LoginResponseDto
            {
                AccessToken = accessToken,
                ExpiresAt = expiresAt,
                Message = "Login successful",
                User = new UserReadDto
                {
                    UserID = user.UserID,
                    Email = user.Email,
                    FullName = user.FullName,
                    Password = user.PasswordHash,
                    IsHotelOwner = user.IsHotelOwner,
                    IsTourAgency = user.IsTourAgency,
                    IsVehicleAgency = user.IsVehicleAgency,
                    IsWebAdmin = user.IsWebAdmin,
                    IsModerator = user.IsModerator,
                    IsActive = user.IsActive,
                    CountWarning = user.CountWarning,
                    CreatedAt = user.CreatedAt,
                    otp_code = user.otp_code,
                    otp_expires = user.otp_expires,
                    is_verified = user.is_verified
                }
            });
        }
    }
}