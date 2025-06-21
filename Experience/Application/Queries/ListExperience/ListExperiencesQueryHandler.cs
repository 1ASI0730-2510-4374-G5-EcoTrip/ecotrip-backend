using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Experience.Application.DTOs;
using Experience.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Experience.Application.Queries.ListExperiences
{
    /// <summary>
    /// Handler for listing experiences with filtering, sorting, and pagination
    /// </summary>
    public class ListExperiencesQueryHandler : IRequestHandler<ListExperiencesQuery, IEnumerable<ExperienceDTO>>
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly ILogger<ListExperiencesQueryHandler> _logger;

        public ListExperiencesQueryHandler(
            IExperienceRepository experienceRepository,
            ILogger<ListExperiencesQueryHandler> logger)
        {
            _experienceRepository = experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ExperienceDTO>> Handle(ListExperiencesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving experiences with filters");

            try
            {
                // Get all experiences from repository
                var experiences = await _experienceRepository.GetAllAsync(cancellationToken);
                
                // Apply filters
                var filteredExperiences = experiences.AsQueryable();
                
                // Filter by location if specified
                if (!string.IsNullOrWhiteSpace(request.LocationFilter))
                {
                    filteredExperiences = filteredExperiences.Where(e => 
                        e.Location.Contains(request.LocationFilter, StringComparison.OrdinalIgnoreCase));
                }
                
                // Filter by price range if specified
                if (request.MinPrice.HasValue)
                {
                    filteredExperiences = filteredExperiences.Where(e => e.Price.Amount >= request.MinPrice.Value);
                }
                
                if (request.MaxPrice.HasValue)
                {
                    filteredExperiences = filteredExperiences.Where(e => e.Price.Amount <= request.MaxPrice.Value);
                }
                
                // Filter by date range if specified
                if (request.FromDate.HasValue)
                {
                    var fromDate = request.FromDate.Value.Date; // Remove time component
                    filteredExperiences = filteredExperiences.Where(e => e.Date >= fromDate);
                }
                
                if (request.ToDate.HasValue)
                {
                    var toDate = request.ToDate.Value.Date.AddDays(1).AddSeconds(-1); // End of day
                    filteredExperiences = filteredExperiences.Where(e => e.Date <= toDate);
                }
                
                // Apply sorting
                IOrderedQueryable<Domain.Entities.Experience> sortedExperiences;
                
                switch (request.SortBy?.ToLowerInvariant())
                {
                    case "price":
                        sortedExperiences = request.SortAscending 
                            ? filteredExperiences.OrderBy(e => e.Price.Amount) 
                            : filteredExperiences.OrderByDescending(e => e.Price.Amount);
                        break;
                    case "title":
                        sortedExperiences = request.SortAscending 
                            ? filteredExperiences.OrderBy(e => e.Title) 
                            : filteredExperiences.OrderByDescending(e => e.Title);
                        break;
                    case "location":
                        sortedExperiences = request.SortAscending 
                            ? filteredExperiences.OrderBy(e => e.Location) 
                            : filteredExperiences.OrderByDescending(e => e.Location);
                        break;
                    case "duration":
                        sortedExperiences = request.SortAscending 
                            ? filteredExperiences.OrderBy(e => e.DurationInDays) 
                            : filteredExperiences.OrderByDescending(e => e.DurationInDays);
                        break;
                    case "createdat":
                        sortedExperiences = request.SortAscending 
                            ? filteredExperiences.OrderBy(e => e.CreatedAt) 
                            : filteredExperiences.OrderByDescending(e => e.CreatedAt);
                        break;
                    case "date":
                    default:
                        sortedExperiences = request.SortAscending 
                            ? filteredExperiences.OrderBy(e => e.Date) 
                            : filteredExperiences.OrderByDescending(e => e.Date);
                        break;
                }
                
                // Apply pagination
                var paginatedExperiences = sortedExperiences
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .ToList();
                
                // Map to DTOs
                var experienceDtos = paginatedExperiences.Select(e => new ExperienceDTO
                {
                    Id = e.Id.ToString(),
                    Title = e.Title,
                    Summary = CreateSummary(e.Description),
                    Date = e.Date,
                    Location = e.Location,
                    DurationInDays = e.DurationInDays,
                    Price = e.Price.Amount,
                    Currency = e.Price.Currency,
                    FormattedPrice = e.Price.ToString(),
                    MainImageUrl = e.MainImageUrl,
                    AgentId = e.AgentId,
                    // AgentName would typically come from a user/agent service
                    Status = e.Status.ToString(),
                    AverageRating = CalculateAverageRating(e.Reviews),
                    ReviewCount = e.Reviews?.Count ?? 0,
                    CreatedAt = e.CreatedAt
                }).ToList();

                _logger.LogInformation("Retrieved {Count} experiences", experienceDtos.Count);
                
                return experienceDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving experiences: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        private string CreateSummary(string description, int maxLength = 150)
        {
            if (string.IsNullOrEmpty(description))
                return string.Empty;
                
            if (description.Length <= maxLength)
                return description;
                
            return description.Substring(0, maxLength).Trim() + "...";
        }

        private double CalculateAverageRating(ICollection<Domain.Entities.Review> reviews)
        {
            if (reviews == null || reviews.Count == 0)
                return 0;

            return reviews.Average(r => r.Rating);
        }
    }
}