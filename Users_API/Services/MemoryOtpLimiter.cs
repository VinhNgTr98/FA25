using Microsoft.Extensions.Caching.Memory;

namespace UserManagement_API.Services
{
    public class MemoryOtpLimiter : IOtpLimiter
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan Cooldown = TimeSpan.FromSeconds(60);
        private const int DailyMax = 5;

        private record State(DateTime LastSentUtc, int CountToday, DateTime DayKeyUtc);

        public MemoryOtpLimiter(IMemoryCache cache) => _cache = cache;

        public Task<bool> CanSendAsync(int userId, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            var key = $"otp:send:{userId}";
            if (!_cache.TryGetValue<State>(key, out var s)) return Task.FromResult(true);

            var okCooldown = (now - s!.LastSentUtc) >= Cooldown;
            var okDaily = (s.DayKeyUtc.Date == now.Date ? s.CountToday < DailyMax : true);
            return Task.FromResult(okCooldown && okDaily);
        }

        public Task RecordSendAsync(int userId, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            var key = $"otp:send:{userId}";
            _cache.TryGetValue<State>(key, out var s);

            if (s is null || s.DayKeyUtc.Date != now.Date)
                s = new State(now, 1, now);
            else
                s = s with { LastSentUtc = now, CountToday = s.CountToday + 1 };

            _cache.Set(key, s, now.Date.AddDays(1) - now); // hết hạn cuối ngày
            return Task.CompletedTask;
        }
    }
}