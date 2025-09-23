using AutoMapper;
using AutoMapper.QueryableExtensions;
using TourManagement.DTOs;
using TourManagement.Model;
using TourManagement.Repositories.Interfaces;
using TourManagement.Services.Interfaces;

namespace TourManagement.Services
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;

        public TourService(ITourRepository tourRepository, IMapper mapper)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        public IQueryable<TourReadDTO> GetAllToursAsync()
        {
            // Dựng IQueryable DTO để OData áp filter/sort/select ở DB
            return _tourRepository.GetAllAsync()
                                  .ProjectTo<TourReadDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<TourReadDTO?> GetTourByIdAsync(Guid id)
        {
            var tour = await _tourRepository.GetByIdAsync(id);
            return tour == null ? null : _mapper.Map<TourReadDTO>(tour);
        }

        public async Task<TourReadDTO> CreateTourAsync(TourCreateDTO tourCreateDto)
        {
            var tour = _mapper.Map<Tour>(tourCreateDto);

            // Nếu entity Tour có CreatedAt/UpdatedAt, set ở đây
            var now = DateTime.UtcNow;
            var propCreated = tour.GetType().GetProperty("CreatedAt");
            var propUpdated = tour.GetType().GetProperty("UpdatedAt");
            propCreated?.SetValue(tour, now);
            propUpdated?.SetValue(tour, now);

            await _tourRepository.AddAsync(tour);
            await _tourRepository.SaveChangesAsync();
            return _mapper.Map<TourReadDTO>(tour);
        }

        public async Task<bool> UpdateTourAsync(TourUpdateDTO tourUpdateDto)
        {
            var existing = await _tourRepository.GetByIdAsync(tourUpdateDto.TourID);
            if (existing == null) return false;

            _mapper.Map(tourUpdateDto, existing);

            // Cập nhật UpdatedAt nếu có
            var propUpdated = existing.GetType().GetProperty("UpdatedAt");
            propUpdated?.SetValue(existing, DateTime.UtcNow);

            _tourRepository.Update(existing);
            return await _tourRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteTourAsync(Guid id)
        {
            var existing = await _tourRepository.GetByIdAsync(id);
            if (existing == null) return false;

            _tourRepository.Delete(existing);
            return await _tourRepository.SaveChangesAsync();
        }
        public async Task<bool> AssignGuideToTourAsync(Guid tourId, Guid tourGuideId)
        {
            var tour = await _tourRepository.GetByIdAsync(tourId);
            if (tour == null) return false;

            tour.TourGuideId = tourGuideId;
            tour.UpdatedAt = DateTime.UtcNow;

            _tourRepository.Update(tour);
            return await _tourRepository.SaveChangesAsync();
        }

    }
}