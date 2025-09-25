using AutoMapper;
using TourManagement.DTOs;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;
using TourManagement.Services.Interfaces;

namespace TourManagement.Services
{
    public class TourMemberService : ITourMemberService
    {
        private readonly ITourMemberRepository _repository;
        private readonly IMapper _mapper;

        public TourMemberService(ITourMemberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TourMemberReadDTO>> GetAllAsync()
        {
            var members = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TourMemberReadDTO>>(members);
        }

        public async Task<TourMemberReadDTO?> GetByIdAsync(Guid id)
        {
            var member = await _repository.GetByIdAsync(id);
            return _mapper.Map<TourMemberReadDTO?>(member);
        }

        public async Task<TourMemberReadDTO> CreateAsync(TourMemberCreateDTO dto)
        {
            var entity = _mapper.Map<TourMember>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<TourMemberReadDTO>(created);
        }

        public async Task<bool> UpdateAsync(TourMemberUpdateDTO dto)
        {
            var entity = _mapper.Map<TourMember>(dto);
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
