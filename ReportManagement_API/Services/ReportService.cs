using AutoMapper;
using ReportManagement_API.DTOs;
using ReportManagement_API.Models;
using ReportManagement_API.Repositories.Interfaces;
using ReportManagement_API.Services.Interfaces;

namespace ReportManagement_API.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReportReadDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ReportReadDTO>>(list);
        }

        public async Task<ReportReadDTO?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ReportReadDTO>(entity);
        }

        public async Task<ReportReadDTO> CreateAsync(ReportCreateDTO dto)
        {
            var entity = _mapper.Map<Report>(dto);
            entity.CreatedDate = DateTime.Now;
            entity.Status = "Pending";
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<ReportReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ReportUpdateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
