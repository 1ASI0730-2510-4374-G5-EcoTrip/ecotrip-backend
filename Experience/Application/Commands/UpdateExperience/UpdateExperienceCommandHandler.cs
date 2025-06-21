using System;
using System.Threading;
using System.Threading.Tasks;
using Experience.Domain.Repositories;
using Experience.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Experience.Application.Commands.UpdateExperience
{
    /// <summary>
    /// Handler for updating an existing experience
    /// </summary>
    public class UpdateExperienceCommandHandler : IRequestHandler<UpdateExperienceCommand, Unit>
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly ILogger<UpdateExperienceCommandHandler> _logger;

        public UpdateExperienceCommandHandler(
            IExperienceRepository experienceRepository,
            ILogger<UpdateExperienceCommandHandler> logger)
        {
            _experienceRepository = experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateExperienceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update experience with ID: {ExperienceId}", request.Id);

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

                // Check if user has permission to update
                if (experience.AgentId != request.AgentId)
                {
                    _logger.LogWarning("Unauthorized update attempt for experience {ExperienceId} by agent {AgentId}", 
                        request.Id, request.AgentId);
                    throw new UnauthorizedAccessException("You are not authorized to update this experience");
                }

                // Update the experience properties
                experience.UpdateTitle(request.Title);
                experience.UpdateDescription(request.Description);
                experience.UpdateDate(request.Date);
                experience.UpdateLocation(request.Location);
                experience.UpdateDuration(request.DurationInDays);
                
                // Update price if changed
                var newPrice = new Money(request.Price, request.Currency);
                if (experience.Price != newPrice)
                {
                    experience.UpdatePrice(newPrice);
                }

                // Save the changes
                await _experienceRepository.UpdateAsync(experience, cancellationToken);

                _logger.LogInformation("Successfully updated experience with ID: {ExperienceId}", request.Id);

                return Unit.Value;
            }
            catch (Exception ex) when (ex is not UnauthorizedAccessException && ex is not ApplicationException)
            {
                _logger.LogError(ex, "Error updating experience {ExperienceId}: {ErrorMessage}", request.Id, ex.Message);
                throw;
            }
        }
    }
}