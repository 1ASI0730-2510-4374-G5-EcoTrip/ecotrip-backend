using System;
using System.Threading;
using System.Threading.Tasks;
using Experience.Domain.Events;
using Experience.Domain.Repositories;
using Experience.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Experience.Application.Commands.CreateExperience
{
    public class CreateExperienceCommandHandler
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly ILogger<CreateExperienceCommandHandler> _logger;

        public CreateExperienceCommandHandler(
            IExperienceRepository experienceRepository,
            ILogger<CreateExperienceCommandHandler> logger)
        {
            _experienceRepository = experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(CreateExperienceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new experience with title: {Title}", request.Title);

            try
            {
                // Validate agent ID is provided
                if (string.IsNullOrEmpty(request.AgentId))
                {
                    throw new ArgumentException("Agent ID is required", nameof(request.AgentId));
                }

                // Create domain entities and value objects
                var experienceId = ExperienceId.Create();
                var price = new Money(request.Price, request.Currency);

                // Create the experience entity
                var experience = new Domain.Entities.Experience(
                    experienceId,
                    request.Title,
                    request.Description,
                    request.Date,
                    request.Location,
                    request.DurationInDays,
                    price,
                    request.MainImageUrl,
                    request.AgentId
                );

                // Persist the entity
                await _experienceRepository.AddAsync(experience, cancellationToken);

                _logger.LogInformation("Successfully created experience with ID: {ExperienceId}", experienceId);                // TODO: Publish domain event when event system is implemented

                return experienceId.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating experience: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}