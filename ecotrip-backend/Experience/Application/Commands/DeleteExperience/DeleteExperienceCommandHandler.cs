using System;
using System.Threading;
using System.Threading.Tasks;
using Experience.Domain.Repositories;
using Experience.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Experience.Application.Commands.DeleteExperience;

/// <summary>
/// Handler for deleting an experience
/// </summary>
public class DeleteExperienceCommandHandler
{
    private readonly IExperienceRepository _experienceRepository;
    private readonly ILogger<DeleteExperienceCommandHandler> _logger;

    public DeleteExperienceCommandHandler(
        IExperienceRepository experienceRepository,
        ILogger<DeleteExperienceCommandHandler> logger)
    {
        _experienceRepository = experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)    {
        _logger.LogInformation("Attempting to delete experience with ID: {ExperienceId}", request.Id);

        try
        {
            // Parse the experience ID
            var experienceId = ExperienceId.Parse(request.Id);

            // Get the experience
            var experience = await _experienceRepository.GetByIdAsync(experienceId, cancellationToken);

            // Check if experience exists
            if (experience == null)
            {
                _logger.LogWarning("Experience with ID {ExperienceId} not found", request.Id);
                throw new ApplicationException($"Experience with ID {request.Id} not found");
            }

            // Check if user has permission to delete
            if (!request.IsAdmin && experience.AgentId != request.AgentId)
            {
                _logger.LogWarning("Unauthorized deletion attempt for experience {ExperienceId} by agent {AgentId}", 
                    request.Id, request.AgentId);
                throw new UnauthorizedAccessException("You are not authorized to delete this experience");
            }

            // Delete the experience
            await _experienceRepository.DeleteAsync(experienceId, cancellationToken);

            _logger.LogInformation("Successfully deleted experience with ID: {ExperienceId}", request.Id);
        }
        catch (Exception ex) when (ex is not UnauthorizedAccessException && ex is not ApplicationException)
        {
            _logger.LogError(ex, "Error deleting experience {ExperienceId}: {ErrorMessage}", request.Id, ex.Message);
            throw;
        }
    }
}