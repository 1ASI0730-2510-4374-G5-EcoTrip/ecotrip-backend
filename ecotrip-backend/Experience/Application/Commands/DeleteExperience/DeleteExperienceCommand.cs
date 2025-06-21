using MediatR;
using System;

namespace Experience.Application.Commands.DeleteExperience
{
    /// <summary>
    /// Command to delete an experience
    /// </summary>
    public class DeleteExperienceCommand : IRequest<Unit>
    {
        /// <summary>
        /// ID of the experience to delete
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ID of the agent requesting deletion
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// Flag indicating if the request comes from an admin user
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}