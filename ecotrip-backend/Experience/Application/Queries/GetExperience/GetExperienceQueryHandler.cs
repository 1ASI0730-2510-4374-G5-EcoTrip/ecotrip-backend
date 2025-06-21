using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Experience.Application.DTOs;
using Experience.Domain.Repositories;
using Experience.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Experience.Application.Queries.GetExperience
{
    /// <summary>
    /// Handler for retrieving a specific experience by ID
    /// </summary>
    public class GetExperienceQueryHandler : IRequestHandler<GetExperienceQuery, ExperienceDetailsDTO>
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly ILogger<GetExperienceQueryHandler> _logger;

        public GetExperienceQueryHandler(
            IExperienceRepository experienceRepository,
            ILogger<GetExperienceQueryHandler> logger)
        {
            _experienceRepository = experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ExperienceDetailsDTO> Handle(GetExperienceQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting experience details for ID: {ExperienceId}", request.Id);

            try
            {
                // Parse the experience ID
                var experienceId = ExperienceId.Parse(request.Id);

                // Get the experience from repository
                var experience = await _experienceRepository.GetByIdAsync(experienceId, cancellationToken);

                // Return null if not found
                if (experience == null)
                {
                    _logger.LogWarning("Experience with ID {ExperienceId} not found", request.Id);
                    return null;
                }

                // Map domain entity to DTO
                var dto = new ExperienceDetailsDTO
                {
                    Id = experience.Id.ToString(),
                    Title = experience.Title,
                    Description = experience.Description,
                    Date = experience.Date,
                    Location = experience.Location,
                    DurationInDays = experience.DurationInDays,
                    Price = experience.Price.Amount,
                    Currency = experience.Price.Currency,
                    FormattedPrice = experience.Price.ToString(),
                    MainImageUrl = experience.MainImageUrl,
                    ImageUrls = experience.ImageUrls,
                    AgentId = experience.AgentId,
                    // AgentName would typically come from a user/agent service
                    Status = experience.Status.ToString(),
                    Reviews = experience.Reviews.Select(r => new ReviewDTO
                    {
                        Id = r.Id.ToString(),
                        UserId = r.UserId,
                        UserName = r.UserName,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        IsVerified = r.IsVerified,
                        CreatedAt = r.CreatedAt
                    }).ToList(),
                    CreatedAt = experience.CreatedAt,
                    UpdatedAt = experience.UpdatedAt
                };

                // Calculate average rating and review count
                if (dto.Reviews != null && dto.Reviews.Any())
                {
                    dto.AverageRating = dto.Reviews.Average(r => r.Rating);
                    dto.ReviewCount = dto.Reviews.Count;
                }
                else
                {
                    dto.AverageRating = 0;
                    dto.ReviewCount = 0;
                }

                _logger.LogInformation("Successfully retrieved experience with ID: {ExperienceId}", request.Id);

                return dto;
            }
            catch (Exception ex) when (ex is not ApplicationException)
            {
                _logger.LogError(ex, "Error retrieving experience {ExperienceId}: {ErrorMessage}", request.Id, ex.Message);
                throw;
            }
        }
    }
}