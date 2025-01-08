using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class OtpRepository(AppDbContext appDbContext) : IRepository<OTP>
{
    private readonly AppDbContext _context = appDbContext;
    public async Task AddAsync(OTP entity)
    {
        await _context.OTPs.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(OTP entity)
    {
        _context.OTPs.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<OTP>> GetAllAsync()
    {
        var otps = await _context.OTPs.AsNoTracking().ToListAsync();
        return otps;
    }

    public async Task<OTP> GetByIdAsync(Guid id)
    {
        var otp = await _context.OTPs.FindAsync(id);
        return otp;
    }

    public async Task UpdateAsync(OTP entity)
    {
        _context.OTPs.Update(entity);
        await _context.SaveChangesAsync();
    }
}
