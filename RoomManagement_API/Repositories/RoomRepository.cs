using Microsoft.EntityFrameworkCore;
using RoomManagement_API.Data;
using RoomManagement_API.Models;
using System;

namespace RoomManagement_API.Repositories.Rooms
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomManagement_APIContext _db;
        public RoomRepository(RoomManagement_APIContext db) => _db = db;

        public Task<Room?> GetByIdAsync(Guid id, CancellationToken ct) =>
            _db.Room.FirstOrDefaultAsync(x => x.RoomId == id, ct);

        public Task<List<Room>> GetAllAsync(CancellationToken ct) =>
            _db.Room.AsNoTracking().OrderBy(x => x.RoomName).ToListAsync(ct);

        public Task<List<Room>> GetByHotelAsync(Guid hotelId, CancellationToken ct) =>
            _db.Room.AsNoTracking()
                     .Where(x => x.HotelId == hotelId)
                     .OrderBy(x => x.RoomName)
                     .ToListAsync(ct);

        public async Task<Room> AddAsync(Room room, CancellationToken ct)
        {
            _db.Room.Add(room);
            await _db.SaveChangesAsync(ct);
            return room;
        }

        public async Task<Room?> UpdateAsync(Room room, CancellationToken ct)
        {
            var exist = await _db.Room.FirstOrDefaultAsync(x => x.RoomId == room.RoomId, ct);
            if (exist == null) return null;

            _db.Entry(exist).CurrentValues.SetValues(room);
            await _db.SaveChangesAsync(ct);
            return exist;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var exist = await _db.Room.FirstOrDefaultAsync(x => x.RoomId == id, ct);
            if (exist == null) return false;

            _db.Room.Remove(exist);
            await _db.SaveChangesAsync(ct);
            return true;
        }
        public async Task<decimal?> GetLowestPriceAsync(CancellationToken ct) =>
       await _db.Room.OrderBy(r => r.Price)
                       .Select(r => (decimal?)r.Price)
                       .FirstOrDefaultAsync(ct);
        public async Task<decimal?> GetHighestPriceAsync(CancellationToken ct) =>
    await _db.Room.OrderByDescending(r => r.Price)
                    .Select(r => (decimal?)r.Price)
                    .FirstOrDefaultAsync(ct);
        public async Task<decimal?> GetLowestPriceByHotelAsync(Guid hotelId, CancellationToken ct)
        {
            return await _db.Room
                .Where(r => r.HotelId == hotelId)
                .OrderBy(r => r.Price)
                .Select(r => (decimal?)r.Price)
                .FirstOrDefaultAsync(ct);
        }
        public async Task<decimal?> GetHighestPriceByHotelAsync(Guid hotelId, CancellationToken ct)
        {
            return await _db.Room
                .Where(r => r.HotelId == hotelId)
                .OrderByDescending(r => r.Price)
                .Select(r => (decimal?)r.Price)
                .FirstOrDefaultAsync(ct);
        }

        public IQueryable<Room> AsQueryable() => _db.Room.AsQueryable();
    }
}
