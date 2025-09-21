using RoomManagement_API.Models;

namespace RoomManagement_API.Repositories.Rooms
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<Room>> GetAllAsync(CancellationToken ct);
        Task<List<Room>> GetByHotelAsync(Guid hotelId, CancellationToken ct);
        Task<Room> AddAsync(Room room, CancellationToken ct);
        Task<Room?> UpdateAsync(Room room, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<decimal?> GetLowestPriceAsync(CancellationToken ct);
        Task<decimal?> GetHighestPriceAsync(CancellationToken ct);
        Task<decimal?> GetLowestPriceByHotelAsync(Guid hotelId, CancellationToken ct);
        Task<decimal?> GetHighestPriceByHotelAsync(Guid hotelId, CancellationToken ct);

        IQueryable<Room> AsQueryable();
    }
}
