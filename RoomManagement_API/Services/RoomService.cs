using AutoMapper;
using RoomManagement_API.DTOs;
using RoomManagement_API.Models;
using RoomManagement_API.Repositories.Rooms;

namespace RoomManagement_API.Services.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repo;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<RoomReadDto?> GetAsync(Guid id, CancellationToken ct)
        {
            var room = await _repo.GetByIdAsync(id, ct);
            return room == null ? null : _mapper.Map<RoomReadDto>(room);
        }

        public async Task<List<RoomReadDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _repo.GetAllAsync(ct);
            return _mapper.Map<List<RoomReadDto>>(list);
        }

        public async Task<List<RoomReadDto>> GetByHotelAsync(Guid hotelId, CancellationToken ct)
        {
            var list = await _repo.GetByHotelAsync(hotelId, ct);
            return _mapper.Map<List<RoomReadDto>>(list);
        }

        public async Task<RoomReadDto> CreateAsync(RoomCreateDto dto, CancellationToken ct)
        {
            if (dto.RoomCapacity <= 0) throw new ArgumentException("RoomCapacity must be > 0");
            if (dto.Price < 0) throw new ArgumentException("Price must be >= 0");

            var entity = _mapper.Map<Room>(dto);
            entity.RoomId = Guid.NewGuid();

            var saved = await _repo.AddAsync(entity, ct);
            return _mapper.Map<RoomReadDto>(saved);
        }

        public async Task<RoomReadDto?> UpdateAsync(RoomUpdateDto dto, CancellationToken ct)
        {
            if (dto.RoomCapacity <= 0) throw new ArgumentException("RoomCapacity must be > 0");
            if (dto.Price < 0) throw new ArgumentException("Price must be >= 0");

            // TODO: xác nhận RoomType / HotelId nếu cho phép đổi

            var entity = _mapper.Map<Room>(dto);
            var saved = await _repo.UpdateAsync(entity, ct);
            return saved == null ? null : _mapper.Map<RoomReadDto>(saved);
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken ct) =>
            _repo.DeleteAsync(id, ct);

        private static void ValidateTimes(DateTime? checkIn, DateTime? checkOut)
        {
            if (checkIn.HasValue && checkOut.HasValue && checkOut <= checkIn)
                throw new ArgumentException("Check-out must be later than check-in");
        }
    }

}
