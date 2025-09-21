using RoomManagement_API.DTOs;
using RoomManagement_API.Models;

namespace RoomManagement_API.Services.Rooms
{
    public interface IRoomService
    {
        Task<RoomReadDto?> GetAsync(Guid id, CancellationToken ct);
        Task<List<RoomReadDto>> GetAllAsync(CancellationToken ct);
        Task<List<RoomReadDto>> GetByHotelAsync(Guid hotelId, CancellationToken ct);
        Task<RoomReadDto> CreateAsync(RoomCreateDto dto, CancellationToken ct);
        Task<RoomReadDto?> UpdateAsync(RoomUpdateDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<decimal?> GetLowestPriceAsync(CancellationToken ct);
        Task<decimal?> GetHighestPriceAsync(CancellationToken ct);
        Task<decimal?> GetLowestPriceByHotelAsync(Guid hotelId, CancellationToken ct);
        Task<decimal?> GetHighestPriceByHotelAsync(Guid hotelId, CancellationToken ct);

        IQueryable<RoomReadDto> AsQueryable();
    }

}
