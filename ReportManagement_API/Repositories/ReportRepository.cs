using Microsoft.EntityFrameworkCore;
using ReportManagement_API.Data;
using ReportManagement_API.Models;
using ReportManagement_API.Repositories.Interfaces;
using System;

namespace ReportManagement_API.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportContext _context;

        public ReportRepository(ReportContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(int id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task AddAsync(Report entity)
        {
            await _context.Reports.AddAsync(entity);
        }

        public async Task UpdateAsync(Report entity)
        {
            _context.Reports.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
