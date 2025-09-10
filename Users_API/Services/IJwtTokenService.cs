namespace UserManagement_API.Services
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(int userId, string username, IEnumerable<string> roles, out DateTime expiresAt);
    }
}