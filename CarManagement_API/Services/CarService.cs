using AutoMapper;
using CarManagement_API.DTOs;
using CarManagement_API.Models;
using CarManagement_API.Repositories.Interfaces;
using CarManagement_API.Services.Interfaces;

namespace CarManagement_API.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _repository;
        private readonly IMapper _mapper;

        public CarService(ICarRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarReadDto>> GetAllAsync()
        {
            var cars = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CarReadDto>>(cars);
        }

        public async Task<CarReadDto?> GetByIdAsync(Guid id)
        {
            var car = await _repository.GetByIdAsync(id);
            return car == null ? null : _mapper.Map<CarReadDto>(car);
        }

        public async Task<CarReadDto> CreateAsync(CarCreateDto dto)
        {
            var car = _mapper.Map<Car>(dto);
            await _repository.AddAsync(car);
            await _repository.SaveChangesAsync();
            return _mapper.Map<CarReadDto>(car);
        }

        public async Task<bool> UpdateAsync(Guid id, CarUpdateDto dto)
        {
            var car = await _repository.GetByIdAsync(id);
            if (car == null)
            {
                return false;
            }

            _mapper.Map(dto, car);
            await _repository.UpdateAsync(car);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var car = await _repository.GetByIdAsync(id);
            if (car == null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CarReadDto>> GetFilteredCarsAsync(
            string? transmission = null,
            string? fuel = null,
            string? carBrand = null,
            string? carName = null,
            int? engineCc = null)
        {
            var cars = await _repository.GetFilteredCarsAsync(transmission, fuel, carBrand, carName, engineCc);
            return _mapper.Map<IEnumerable<CarReadDto>>(cars);
        }
    }
}
