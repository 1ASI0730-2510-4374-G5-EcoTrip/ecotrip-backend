namespace Experience.Application.Commands.DeleteExperience;

/// <summary>
/// Command to delete an experience
/// </summary>
public class DeleteExperienceCommand
{
    /// <summary>
    /// ID of the experience to delete
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// ID of the agent requesting deletion
    /// </summary>
    public required string AgentId { get; set; }    /// <summary>
    /// Flag indicating if the request comes from an admin user
    /// </summary>
    public bool IsAdmin { get; set; }
}