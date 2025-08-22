using HotelsManagement_API.DTOs;

namespace HotelsManagement_API.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelReadDto>> GetAllAsync();
        Task<HotelReadDto?> GetByIdAsync(Guid id);
        Task<HotelReadDto> AddAsync(HotelReadDto hotelDto);
        Task<HotelReadDto?> UpdateAsync(HotelReadDto hotelDto);
        Task<bool> DeleteAsync(Guid id);
    }

}
