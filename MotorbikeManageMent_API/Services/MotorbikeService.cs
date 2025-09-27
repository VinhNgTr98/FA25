using MotorbikeManageMent_API.DTOs;
using MotorbikeManageMent_API.Repositories.Interfaces;
using MotorbikeManageMent_API.Services.Interfaces;
using MotorbikeManageMent_API.Models;
using AutoMapper;

namespace MotorbikeManageMent_API.Services
{
    public class MotorbikeService : IMotorbikeService
    {
        private readonly IMotorbikeRepository _repository;
        private readonly IMapper _mapper;

        public MotorbikeService(IMotorbikeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MotorbikeReadDto>> GetAllAsync()
        {
            var motorbikes = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MotorbikeReadDto>>(motorbikes);
        }

        public async Task<MotorbikeReadDto?> GetByIdAsync(Guid id)
        {
            var motorbike = await _repository.GetByIdAsync(id);
            return motorbike == null ? null : _mapper.Map<MotorbikeReadDto>(motorbike);
        }

        public async Task<MotorbikeReadDto> CreateAsync(MotorbikeCreateDto dto)
        {
            var motorbike = _mapper.Map<Motorbike>(dto);
            await _repository.AddAsync(motorbike);
            await _repository.SaveChangesAsync();
            return _mapper.Map<MotorbikeReadDto>(motorbike);
        }

        public async Task<bool> UpdateAsync(Guid id, MotorbikeUpdateDto dto)
        {
            var motorbike = await _repository.GetByIdAsync(id);
            if (motorbike == null)
            {
                return false;
            }

            _mapper.Map(dto, motorbike);
            await _repository.UpdateAsync(motorbike);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var motorbike = await _repository.GetByIdAsync(id);
            if (motorbike == null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return true;
        }

        // Phương thức lọc
        public async Task<IEnumerable<MotorbikeReadDto>> GetFilteredMotorbikesAsync(
            string? transmission = null,
            string? fuel = null,
            string? motorbikeBrand = null,
            string? motorbikeName = null,
            int? engineCc = null)
        {
            var motorbikes = await _repository.GetFilteredMotorbikesAsync(transmission, fuel, motorbikeBrand, motorbikeName, engineCc);
            return _mapper.Map<IEnumerable<MotorbikeReadDto>>(motorbikes);
        }
        // Lấy xe theo VehicleAgencyId
        public async Task<IEnumerable<MotorbikeReadDto>> GetByVehicleAgencyIdAsync(Guid vehicleAgencyId)
        {
            var motorbikes = await _repository.GetByVehicleAgencyIdAsync(vehicleAgencyId);
            return _mapper.Map<IEnumerable<MotorbikeReadDto>>(motorbikes);
        }

    }
}
