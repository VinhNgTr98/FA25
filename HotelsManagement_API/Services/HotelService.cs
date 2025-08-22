using HotelsManagement_API.DTOs;
using HotelsManagement_API.Models;
using HotelsManagement_API.Repositories;
using AutoMapper;

namespace HotelsManagement_API.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _repo;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HotelReadDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<HotelReadDto>>(entities);
        }

        public async Task<HotelReadDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<HotelReadDto>(entity);
        }

        public async Task<HotelReadDto> AddAsync(HotelReadDto dto)
        {
            var entity = _mapper.Map<Hotel>(dto);
            var result = await _repo.AddAsync(entity);
            return _mapper.Map<HotelReadDto>(result);
        }

        public async Task<HotelReadDto?> UpdateAsync(HotelReadDto dto)
        {
            var entity = _mapper.Map<Hotel>(dto);
            var result = await _repo.UpdateAsync(entity);
            return result == null ? null : _mapper.Map<HotelReadDto>(result);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
