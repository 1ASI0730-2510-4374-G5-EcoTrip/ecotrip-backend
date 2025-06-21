using backendPrueva.Tourists.Domain;
using backendPrueva.Tourists.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backendPrueva.Tourists.Infrastructure.EF;

public class TouristRepository : ITouristRepository
{
    private readonly EcoTripDbContext _context;

    public TouristRepository(EcoTripDbContext context)
    {
        _context = context;
    }

    public async Task<Tourist?> GetByEmailAsync(string email)
    {
        return await _context.Tourists.FirstOrDefaultAsync(t => t.Email == email);
    }

    public async Task AddAsync(Tourist tourist)
    {
        await _context.Tourists.AddAsync(tourist);
        await _context.SaveChangesAsync();
    }
}