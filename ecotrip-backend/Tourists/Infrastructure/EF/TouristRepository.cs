using ecotrip_backend.Tourists.Domain;
using ecotrip_backend.Tourists.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ecotrip_backend.Tourists.Infrastructure.EF;

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

    public async Task<Tourist?> GetByIdAsync(Guid id)
    {
        return await _context.Tourists.FindAsync(id);
    }

    public async Task<IEnumerable<Tourist>> GetAllAsync()
    {
        return await _context.Tourists.ToListAsync();
    }

    public async Task AddAsync(Tourist tourist)
    {
        await _context.Tourists.AddAsync(tourist);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tourist tourist)
    {
        _context.Tourists.Update(tourist);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var tourist = await GetByIdAsync(id);
        if (tourist != null)
        {
            _context.Tourists.Remove(tourist);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _context.Tourists.AnyAsync(t => t.Id == id);
    }
}