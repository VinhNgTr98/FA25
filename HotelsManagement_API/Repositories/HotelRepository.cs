using HotelsManagement_API.Models;
using HotelsManagement_API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagement_API.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelsManagement_APIContext _context;
        public HotelRepository(HotelsManagement_APIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotel.ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(Guid id)
        {
            return await _context.Hotel.FindAsync(id);
        }

        public async Task<Hotel> AddAsync(Hotel hotel)
        {
            _context.Hotel.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        public async Task<Hotel?> UpdateAsync(Hotel hotel)
        {
            var existing = await _context.Hotel.FindAsync(hotel.HotelId);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(hotel);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var hotel = await _context.Hotel.FindAsync(id);
            if (hotel == null) return false;
            _context.Hotel.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
