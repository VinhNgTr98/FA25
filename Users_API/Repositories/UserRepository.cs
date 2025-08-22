using Microsoft.EntityFrameworkCore;
using System;
using User_API.Models;
using Users_API.Data;

namespace User_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Users_APIContext _db;
        public UserRepository(Users_APIContext db) => _db = db;

        public Task<User?> GetByIdAsync(int id, CancellationToken ct) =>
            _db.User.FirstOrDefaultAsync(x => x.UserID == id, ct);

        public Task<List<User>> GetAllAsync(CancellationToken ct) =>
            _db.User.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(ct);

        public async Task<User> AddAsync(User user, CancellationToken ct)
        {
            _db.User.Add(user);
            await _db.SaveChangesAsync(ct);
            return user;
        }

        public async Task<User?> UpdateAsync(User user, CancellationToken ct)
        {
            var exist = await _db.User.FirstOrDefaultAsync(x => x.UserID == user.UserID, ct);
            if (exist == null) return null;

            // Chỉ set các trường cho phép
            exist.UsersName = user.UsersName;
            exist.IsHotelOwner = user.IsHotelOwner;
            exist.IsTourAgency = user.IsTourAgency;
            exist.IsVehicleAgency = user.IsVehicleAgency;
            exist.IsWebAdmin = user.IsWebAdmin;
            exist.IsSupervisor = user.IsSupervisor;

            // IsActive có thể null trong UpdateDto -> nếu muốn cho update:
            exist.IsActive = user.IsActive;

            await _db.SaveChangesAsync(ct);
            return exist;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var u = await _db.User.FirstOrDefaultAsync(x => x.UserID == id, ct);
            if (u == null) return false;
            _db.User.Remove(u); // hoặc soft delete: u.IsActive=false;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
