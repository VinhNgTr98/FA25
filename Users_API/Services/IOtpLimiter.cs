namespace UserManagement_API.Services
{
    public interface IOtpLimiter
    {
        Task<bool> CanSendAsync(int userId, CancellationToken ct = default);
        Task RecordSendAsync(int userId, CancellationToken ct = default);
    }
}
