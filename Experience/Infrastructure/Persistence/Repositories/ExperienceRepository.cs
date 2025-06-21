using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Experience.Domain.Enum;
using Experience.Domain.Repositories;
using Experience.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Experience.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for Experience entities
    /// </summary>
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly ExperienceDbContext _context;
        private readonly ILogger<ExperienceRepository> _logger;

        public ExperienceRepository(ExperienceDbContext context, ILogger<ExperienceRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<Domain.Entities.Experience> GetByIdAsync(ExperienceId id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting experience with ID: {ExperienceId}", id);
            
            return await _context.Experiences
                .Include(e => e.Reviews)
                .FirstOrDefaultAsync(e => e.Id.Value == id.Value, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Domain.Entities.Experience>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting all experiences");
            
            return await _context.Experiences
                .Include(e => e.Reviews)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Domain.Entities.Experience>> GetByAgentIdAsync(string agentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting experiences for agent: {AgentId}", agentId);
            
            return await _context.Experiences
                .Include(e => e.Reviews)
                .Where(e => e.AgentId == agentId)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Domain.Entities.Experience>> GetByStatusAsync(ExperienceStatus status, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting experiences with status: {Status}", status);
            
            return await _context.Experiences
                .Include(e => e.Reviews)
                .Where(e => e.Status == status)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddAsync(Domain.Entities.Experience experience, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Adding new experience with ID: {ExperienceId}", experience.Id);
            
            await _context.Experiences.AddAsync(experience, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Domain.Entities.Experience experience, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating experience with ID: {ExperienceId}", experience.Id);
            
            // Attach and mark as modified
            _context.Entry(experience).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict when updating experience {ExperienceId}", experience.Id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeleteAsync(ExperienceId id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting experience with ID: {ExperienceId}", id);
            
            var experience = await _context.Experiences.FindAsync(new object[] { id.Value }, cancellationToken);
            
            if (experience == null)
            {
                _logger.LogWarning("Experience with ID {ExperienceId} not found for deletion", id);
                return;
            }
            
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}