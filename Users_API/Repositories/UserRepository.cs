using Microsoft.EntityFrameworkCore;
using System;
using User_API.Models;
using Users_API.Data;

namespace User_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Users_APIContext _ctx;
        public UserRepository(Users_APIContext ctx) => _ctx = ctx;

        public async Task<List<User>> GetAllAsync(CancellationToken ct = default) =>
            await _ctx.User.AsNoTracking().ToListAsync(ct);

        public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await _ctx.User.FirstOrDefaultAsync(u => u.UserID == id, ct);

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
            await _ctx.User.FirstOrDefaultAsync(u => u.Email == email, ct);

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
            await _ctx.User.AnyAsync(u => u.Email == email, ct);

        public async Task<User> CreateAsync(User user, CancellationToken ct = default)
        {
            await _ctx.User.AddAsync(user, ct);
            await _ctx.SaveChangesAsync(ct);
            return user;
        }

        public async Task<User> UpdateAsync(User user, CancellationToken ct = default)
        {
            _ctx.User.Update(user);
            await _ctx.SaveChangesAsync(ct);
            return user;
        }

        public async Task DeleteAsync(User user, CancellationToken ct = default)
        {
            _ctx.User.Remove(user);
            await _ctx.SaveChangesAsync(ct);
        }
    }

}

