using Experience.Domain.Entities;
using Experience.Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Experience.Infrastructure.Persistence
{
    public class ExperienceDbContext : DbContext
    {
        public DbSet<Domain.Entities.Experience> Experiences { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public ExperienceDbContext(DbContextOptions<ExperienceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}