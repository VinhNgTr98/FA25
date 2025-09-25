using AutoMapper;
using TourManagement.DTOs;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;
using TourManagement.Services.Interfaces;

namespace TourManagement.Services
{
    public class TourGuideService : ITourGuideService
    {
        private readonly ITourGuideRepository _repository;
        private readonly IMapper _mapper;

        public TourGuideService(ITourGuideRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TourGuideReadDTO>> GetAllAsync()
        {
            var guides = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TourGuideReadDTO>>(guides);
        }

        public async Task<TourGuideReadDTO?> GetByIdAsync(Guid id)
        {
            var guide = await _repository.GetByIdAsync(id);
            return _mapper.Map<TourGuideReadDTO?>(guide);
        }

        public async Task<TourGuideReadDTO> CreateAsync(TourGuideCreateDTO dto)
        {
            var entity = _mapper.Map<TourGuide>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<TourGuideReadDTO>(created);
        }

        public async Task<bool> UpdateAsync(TourGuideUpdateDTO dto)
        {
            var entity = _mapper.Map<TourGuide>(dto);
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
