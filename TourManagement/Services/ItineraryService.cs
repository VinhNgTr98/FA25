using AutoMapper;
using TourManagement.DTOs;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;
using TourManagement.Services.Interfaces;

namespace TourManagement.Services
{
    public class ItineraryService : IItineraryService
    {
        private readonly IItineraryRepository _repository;
        private readonly IMapper _mapper;

        public ItineraryService(IItineraryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItineraryReadDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItineraryReadDTO>>(items);
        }

        public async Task<ItineraryReadDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ItineraryReadDTO>(entity);
        }

        public async Task<IEnumerable<ItineraryReadDTO>> GetByTourIdAsync(Guid tourId)
        {
            var items = await _repository.GetByTourIdAsync(tourId);
            return _mapper.Map<IEnumerable<ItineraryReadDTO>>(items);
        }

        public async Task<ItineraryReadDTO> CreateAsync(ItineraryCreateDTO dto)
        {
            var entity = _mapper.Map<Itinerary>(dto);
            var created = await _repository.CreateAsync(entity);
            return _mapper.Map<ItineraryReadDTO>(created);
        }

        public async Task<bool> UpdateAsync(Guid id, ItineraryUpdateDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            return await _repository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
