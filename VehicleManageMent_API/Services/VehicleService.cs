using AutoMapper;
using VehicleManageMent_API.DTOs;
using VehicleManageMent_API.Models;
using VehicleManageMent_API.Repositories.Interfaces;
using VehicleManageMent_API.Services.Interfaces;

namespace VehicleManageMent_API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repo;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleReadDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            var mappedList = _mapper.Map<IEnumerable<VehicleReadDTO>>(list);

            // Format Price for each item
            foreach (var item in mappedList)
            {
                item.Price = Math.Round(item.Price, 2); // Round to 2 decimal places
            }

            return mappedList;
        }

        public async Task<VehicleReadDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            var mappedEntity = _mapper.Map<VehicleReadDTO>(entity);
            mappedEntity.Price = Math.Round(mappedEntity.Price, 2); // Round to 2 decimal places

            return mappedEntity;
        }

        public async Task<VehicleReadDTO> CreateAsync(VehicleCreateDTO dto)
        {
            var entity = _mapper.Map<Vehicle>(dto);
            entity.VehiclesID = Guid.NewGuid(); // PK generate
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<VehicleReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, VehicleUpdateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            // Ensure all fields are mapped correctly
            entity.Name = dto.Name;
            entity.VehicleType = dto.VehicleType;
            entity.AvailabilityStatus = dto.AvailabilityStatus;
            entity.Description = dto.Description;
            entity.LicensePlate = dto.LicensePlate;
            entity.ImageUrl = dto.ImageUrl;
            entity.Price = dto.Price;

            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
