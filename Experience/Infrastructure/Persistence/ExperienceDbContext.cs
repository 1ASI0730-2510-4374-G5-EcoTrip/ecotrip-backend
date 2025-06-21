using Experience.Domain.Entities;
using Experience.Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Experience.Infrastructure.Persistence
{
    /// <summary>
    /// Database context for the Experience bounded context
    /// </summary>
    public class ExperienceDbContext : DbContext
    {
        /// <summary>
        /// Experiences DB set
        /// </summary>
        public DbSet<Domain.Entities.Experience> Experiences { get; set; }
        
        /// <summary>
        /// Reviews DB set
        /// </summary>
        public DbSet<Review> Reviews { get; set; }

        public ExperienceDbContext(DbContextOptions<ExperienceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configuration from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            // Alternatively, apply specific configurations:
            // modelBuilder.ApplyConfiguration(new ExperienceConfiguration());
            // modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}