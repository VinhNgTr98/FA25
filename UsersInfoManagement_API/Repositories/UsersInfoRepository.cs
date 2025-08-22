using Microsoft.EntityFrameworkCore;
using UsersInfoManagement_API.Data;
using UsersInfoManagement_API.Models;

namespace UsersInfoManagement_API.Repositories
{
    public class UsersInfoRepository : IUsersInfoRepository
    {
        private readonly UsersInfoManagement_APIContext _db;
        public UsersInfoRepository(UsersInfoManagement_APIContext db) => _db = db;

        public async Task<IEnumerable<UsersInfo>> GetAllAsync(CancellationToken ct = default)
            => await _db.UsersInfo.AsNoTracking().ToListAsync(ct);

        public Task<UsersInfo?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.UsersInfo.FirstOrDefaultAsync(x => x.UsersInfoID == id, ct);

        public Task<UsersInfo?> GetByUserIdAsync(int usersId, CancellationToken ct = default)
            => _db.UsersInfo.AsNoTracking().FirstOrDefaultAsync(x => x.UsersID == usersId, ct);

        public Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken ct = default)
            => _db.UsersInfo.AnyAsync(x => x.Email == email && (excludeId == null || x.UsersInfoID != excludeId), ct);

        public Task<bool> UserExistsAsync(int usersId, CancellationToken ct = default)
            => _db.Users.AnyAsync(u => u.UserID == usersId, ct); // map tới Users có sẵn

        public async Task<UsersInfo> AddAsync(UsersInfo entity, CancellationToken ct = default)
        {
            _db.UsersInfo.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(UsersInfo entity, CancellationToken ct = default)
        {
            _db.UsersInfo.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(UsersInfo entity, CancellationToken ct = default)
        {
            _db.UsersInfo.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
