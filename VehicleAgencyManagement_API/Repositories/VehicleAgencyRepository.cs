using Microsoft.EntityFrameworkCore;
using System;
using VehicleAgencyManagement_API.Data;
using VehicleAgencyManagement_API.Models;
using VehicleAgencyManagement_API.Repositories.Interfaces;

namespace VehicleAgencyManagement_API.Repositories
{
    public class VehicleAgencyRepository : IVehicleAgencyRepository
    {
        private readonly VehicleAgencyContext _context;

        public VehicleAgencyRepository(VehicleAgencyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VehicleAgency>> GetAllAsync()
        {
            return await _context.VehicleAgencies.ToListAsync();
        }

        public async Task<VehicleAgency?> GetByIdAsync(Guid id)
        {
            return await _context.VehicleAgencies.FindAsync(id);
        }

        public async Task AddAsync(VehicleAgency entity)
        {
            await _context.VehicleAgencies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleAgency entity)
        {
            _context.VehicleAgencies.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(VehicleAgency entity)
        {
            _context.VehicleAgencies.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
