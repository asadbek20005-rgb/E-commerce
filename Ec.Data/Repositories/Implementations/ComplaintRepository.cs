using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations
{
    public class ComplaintRepository(AppDbContext appDbContext) : IRepository<Complaint>
    {
        private readonly AppDbContext _context = appDbContext;
        public async Task AddAsync(Complaint entity)
        {
            await _context.Complaints.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Complaint entity)
        {
            _context.Complaints.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Complaint>> GetAllAsync()
        {
            var complaints = await _context.Complaints.AsNoTracking().ToListAsync();
            return complaints;
        }

        public async Task<Complaint> GetByIdAsync(Guid id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            return complaint;
        }

        public async Task UpdateAsync(Complaint entity)
        {
            _context.Complaints.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
