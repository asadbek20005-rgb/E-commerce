using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class AddressRepository(AppDbContext appDbContext) : IAddressRepository
{
    private readonly AppDbContext _context = appDbContext;

    public async Task AddAsync(Address entity)
    {
        await _context.Address.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Address entity)
    {
        _context.Address.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Address> GetAddressByName(Guid sellerId,string address)
    {
        var addressEnitiy = await _context.Address
            .AsNoTracking()
            .Where(x => x.SellerId == sellerId && x.Name == address)
            .FirstOrDefaultAsync();
        return addressEnitiy;
    }

    public async Task<Address> GetAddressBySellerId(Guid sellerId)
    {
        var address = await _context.Address
            .AsNoTracking()
            .Where(x => x.SellerId == sellerId)
            .FirstOrDefaultAsync();

        return address;
    }

    public async Task<List<Address>> GetAllAsync()
    {
        var addresses = await _context.Address.AsNoTracking().ToListAsync();
        return addresses;
    }

    public async Task<Address> GetByIdAsync(Guid id)
    {
        var address = await _context.Address.FindAsync(id);
        return address;
    }

    public async Task UpdateAsync(Address entity)
    {
        _context.Address.Update(entity);
        await _context.SaveChangesAsync();
    }
}