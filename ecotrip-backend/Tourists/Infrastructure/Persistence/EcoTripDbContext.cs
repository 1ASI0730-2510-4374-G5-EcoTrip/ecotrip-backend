using ecotrip_backend.Tourists.Domain;
using Microsoft.EntityFrameworkCore;

namespace ecotrip_backend.Tourists.Infrastructure.Persistence;

public class EcoTripDbContext : DbContext
{
    public EcoTripDbContext(DbContextOptions<EcoTripDbContext> options) : base(options) { }

    public DbSet<Tourist> Tourists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tourist>().HasKey(t => t.Id);
        modelBuilder.Entity<Tourist>().HasIndex(t => t.Email).IsUnique();
    }
}