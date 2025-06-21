using ecotrip_backend.Auth.Domain.Aggregates;
using ecotrip_backend.Auth.Domain.Repositories.Interfaces;
using ecotrip_backend.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ecotrip_backend.Auth.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
    }

    public async Task CreateAsync(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _context.Set<User>()
            .AnyAsync(u => u.Email == email.ToLowerInvariant());
    }
}