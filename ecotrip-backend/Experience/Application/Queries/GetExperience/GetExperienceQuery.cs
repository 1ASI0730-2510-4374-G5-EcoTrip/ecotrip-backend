using Experience.Application.DTOs;
using MediatR;

namespace Experience.Application.Queries.GetExperience
{
    /// <summary>
    /// Query to get a specific experience by ID
    /// </summary>
    public class GetExperienceQuery : IRequest<ExperienceDetailsDTO>
    {
        /// <summary>
        /// ID of the experience to retrieve
        /// </summary>
        public string Id { get; set; }
    }
}