using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using User_API.Repositories;
using UserManagement_API.DTOs;
using UserManagement_API.Services;

namespace Users_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.UsersName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Username and password are required" });

            var user = await _repo.GetByUsernameAsync(dto.UsersName.Trim(), ct);
            if (user == null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid credentials" });

            // build roles từ flags
            var roles = new List<string>();
            if (user.IsWebAdmin) roles.Add("Admin");
            if (user.IsSupervisor) roles.Add("Supervisor");
            if (user.IsHotelOwner) roles.Add("HotelOwner");
            if (user.IsTourAgency) roles.Add("TourAgency");
            if (user.IsVehicleAgency) roles.Add("VehicleAgency");

            var accessToken = _jwt.CreateAccessToken(user.UserID, user.UsersName, roles, out var expiresAt);

            return Ok(new LoginResponseDto
            {
                Message = "Login successful",
                AccessToken = accessToken,
                ExpiresAt = expiresAt,
                Roles = roles 
            });
        }
    }
}
