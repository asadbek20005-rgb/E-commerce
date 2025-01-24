using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;
    public async Task AddAsync(User entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        var users = await _context.Users.AsNoTracking().ToListAsync();
        return users;
    }

 

    public async Task<User> GetUserById(Guid userId)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
        return user;
    }

    public async Task<User> GetUserByPhoneNumber(string phoneNumber)
    {
        var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        return user;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Username == username);
        return user;
    }

    public async Task UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }
}
