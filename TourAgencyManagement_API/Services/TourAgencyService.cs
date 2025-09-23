using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<TourAgencyReadDto>> GetAllAsync()
        {
            var agencies = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TourAgencyReadDto>>(agencies);
        }

        public async Task<TourAgencyReadDto?> GetByIdAsync(Guid id)
        {
            var agency = await _repository.GetByIdAsync(id);
            return _mapper.Map<TourAgencyReadDto?>(agency);
        }

        public async Task<TourAgencyReadDto> CreateAsync(TourAgencyCreateDto dto)
        {
            var agency = _mapper.Map<TourAgency>(dto);
            agency.TourAgencyId = Guid.NewGuid();
            agency.CreatedAt = DateTime.UtcNow;

            var created = await _repository.AddAsync(agency);
            return _mapper.Map<TourAgencyReadDto>(created);
        }

        public async Task<bool> UpdateAsync(Guid id, TourAgencyUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

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
        public async Task<TourAgencyReadDto?> GetByUserIdAsync(int userId)
        {
            var agency = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<TourAgencyReadDto?>(agency);
        }


    }
}