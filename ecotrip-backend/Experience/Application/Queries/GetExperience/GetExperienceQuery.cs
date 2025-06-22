using Experience.Application.DTOs;

namespace Experience.Application.Queries.GetExperience;

/// <summary>
/// Query to get a specific experience by ID
/// </summary>
public class GetExperienceQuery
{
    /// <summary>
    /// ID of the experience to retrieve
    /// </summary>
    public required string Id { get; set; }
}