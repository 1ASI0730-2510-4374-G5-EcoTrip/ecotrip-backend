using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Experience.Domain.Entities;
using Experience.Domain.ValueObjects;
using Experience.Domain.Enums;

namespace Experience.Domain.Repositories
{
    public interface IExperienceRepository
    {
        /// <summary>
        /// Gets an experience by its ID
        /// </summary>
        /// <param name="id">The experience ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The experience if found, null otherwise</returns>
        Task<Entities.Experience?> GetByIdAsync(ExperienceId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all experiences
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of all experiences</returns>
        Task<IEnumerable<Entities.Experience>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all experiences for a specific agent
        /// </summary>
        /// <param name="agentId">The agent ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of experiences for the agent</returns>
        Task<IEnumerable<Entities.Experience>> GetByAgentIdAsync(string agentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets experiences by status
        /// </summary>
        /// <param name="status">The experience status</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of experiences with the specified status</returns>
        Task<IEnumerable<Entities.Experience>> GetByStatusAsync(ExperienceStatus status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new experience
        /// </summary>
        /// <param name="experience">The experience to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task AddAsync(Entities.Experience experience, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing experience
        /// </summary>
        /// <param name="experience">The experience to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateAsync(Entities.Experience experience, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an experience by its ID
        /// </summary>
        /// <param name="id">The ID of the experience to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(ExperienceId id, CancellationToken cancellationToken = default);
    }
}