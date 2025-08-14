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
            var books = _tourRepository.GetAllAsync();
            return books.ProjectTo<TourReadDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<TourReadDTO?> GetTourByIdAsync(Guid id)
        {
            var book = await _tourRepository.GetByIdAsync(id);
            if (book == null) return null;
            return _mapper.Map<TourReadDTO>(book);
        }

        public async Task<TourReadDTO> CreateTourAsync(TourCreateDTO tourCreateDto)
        {
            var book = _mapper.Map<Tour>(tourCreateDto);
            await _tourRepository.AddAsync(book);
            await _tourRepository.SaveChangesAsync();
            return _mapper.Map<TourReadDTO>(book);
        }

        public async Task<bool> UpdateTourAsync(TourUpdateDTO tourUpdateDto)
        {
            var existingTour = await _tourRepository.GetByIdAsync(tourUpdateDto.TourID);
            if (existingTour == null) return false;

            _mapper.Map(tourUpdateDto, existingTour);
            _tourRepository.Update(existingTour);
            return await _tourRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteTourAsync(Guid id)
        {
            var existingTour = await _tourRepository.GetByIdAsync(id);
            if (existingTour == null) return false;

            _tourRepository.Delete(existingTour);
            return await _tourRepository.SaveChangesAsync();
        }
    }
}
