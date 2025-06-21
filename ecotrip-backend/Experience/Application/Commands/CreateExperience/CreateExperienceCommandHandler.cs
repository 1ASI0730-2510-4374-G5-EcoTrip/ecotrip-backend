using System;
using System.Threading;
using System.Threading.Tasks;
using Experience.Domain.Event;
using Experience.Domain.Repositories;
using Experience.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Experience.Application.Commands.CreateExperience
{
    /// <summary>
    /// Handler for creating a new experience
    /// </summary>
    public class CreateExperienceCommandHandler : IRequestHandler<CreateExperienceCommand, string>
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateExperienceCommandHandler> _logger;

        public CreateExperienceCommandHandler(
            IExperienceRepository experienceRepository,
            IMediator mediator,
            ILogger<CreateExperienceCommandHandler> logger)
        {
            _experienceRepository = experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(CreateExperienceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new experience with title: {Title}", request.Title);

            try
            {
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

                // Publish domain event
                await _mediator.Publish(new ExperienceCreatedEvent(
                    experienceId,
                    request.Title,
                    request.Date,
                    request.Location,
                    request.DurationInDays,
                    request.AgentId
                ), cancellationToken);

                _logger.LogInformation("Successfully created experience with ID: {ExperienceId}", experienceId);

                // Return the ID as a string
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