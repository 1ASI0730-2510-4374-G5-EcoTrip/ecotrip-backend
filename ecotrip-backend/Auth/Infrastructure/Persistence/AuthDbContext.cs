using ecotrip_backend.Auth.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace ecotrip_backend.Auth.Infrastructure.Persistence;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<TouristUser> TouristUsers { get; set; }
    public DbSet<AgencyUser> AgencyUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure User as a table-per-hierarchy model
        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasDiscriminator<string>("UserType")
            .HasValue<TouristUser>("Tourist")
            .HasValue<AgencyUser>("Agency");

        modelBuilder.Entity<User>()
            .HasKey(u => u.Email);
            
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();
            
        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();
    }
}