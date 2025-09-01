using AutoMapper;
using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Models;
using TourAgencyManagement_API.Repositories.Interfaces;
using TourAgencyManagement_API.Services.Interfaces;

namespace TourAgencyManagement_API.Services
{
    public class TourAgencyService : ITourAgencyService
    {
        private readonly ITourAgencyRepository _repository;
        private readonly IMapper _mapper;

        public TourAgencyService(ITourAgencyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TourAgencyReadDTO>> GetAllAsync()
        {
            var agencies = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TourAgencyReadDTO>>(agencies);
        }

        public async Task<TourAgencyReadDTO?> GetByIdAsync(Guid id)
        {
            var agency = await _repository.GetByIdAsync(id);
            return _mapper.Map<TourAgencyReadDTO?>(agency);
        }

        public async Task<TourAgencyReadDTO> CreateAsync(TourAgencyCreateDTO dto)
        {
            var agency = _mapper.Map<TourAgency>(dto);
            await _repository.AddAsync(agency);
            return _mapper.Map<TourAgencyReadDTO>(agency);
        }

        public async Task<bool> UpdateAsync(Guid id, TourAgencyUpdateDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var agency = await _repository.GetByIdAsync(id);
            if (agency == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
