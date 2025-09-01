using AutoMapper;
using VehicleAgencyManagement_API.DTOs;
using VehicleAgencyManagement_API.Models;
using VehicleAgencyManagement_API.Repositories.Interfaces;
using VehicleAgencyManagement_API.Services.Interfaces;

namespace VehicleAgencyManagement_API.Services
{
    public class VehicleAgencyService : IVehicleAgencyService
    {
        private readonly IVehicleAgencyRepository _repository;
        private readonly IMapper _mapper;

        public VehicleAgencyService(IVehicleAgencyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleAgencyReadDTO>> GetAllAsync()
        {
            var agencies = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<VehicleAgencyReadDTO>>(agencies);
        }

        public async Task<VehicleAgencyReadDTO?> GetByIdAsync(Guid id)
        {
            var agency = await _repository.GetByIdAsync(id);
            return agency == null ? null : _mapper.Map<VehicleAgencyReadDTO>(agency);
        }

        public async Task<VehicleAgencyReadDTO> CreateAsync(VehicleAgencyCreateDTO dto)
        {
            var entity = _mapper.Map<VehicleAgency>(dto);
            entity.VehicleAgencyId = Guid.NewGuid();

            await _repository.AddAsync(entity);
            return _mapper.Map<VehicleAgencyReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, VehicleAgencyUpdateDTO dto)
        {
            var agency = await _repository.GetByIdAsync(id);
            if (agency == null) return false;

            _mapper.Map(dto, agency);
            await _repository.UpdateAsync(agency);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var agency = await _repository.GetByIdAsync(id);
            if (agency == null) return false;

            await _repository.DeleteAsync(agency);
            return true;
        }
    }
}
