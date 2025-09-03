using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserManagement_API.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _cfg;
        public JwtTokenService(IConfiguration cfg) => _cfg = cfg;

        public string CreateAccessToken(int userId, string username, IEnumerable<string> roles, out DateTime expiresAt)
        {
            var jwt = _cfg.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, username),
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var minutes = int.Parse(jwt["AccessTokenMinutes"]!);
            expiresAt = DateTime.UtcNow.AddMinutes(minutes);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
