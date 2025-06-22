using System;
using Experience.Domain.ValueObjects;

namespace Experience.Domain.Events
{
    /// <summary>
    /// Event raised when an experience is published
    /// </summary>
    public class ExperiencePublishedEvent
    {
        /// <summary>
        /// ID of the experience that was published
        /// </summary>
        public ExperienceId ExperienceId { get; }
        
        /// <summary>
        /// Title of the experience
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// Date when the experience will take place
        /// </summary>
        public DateTime ExperienceDate { get; }
        
        /// <summary>
        /// ID of the agent who published the experience
        /// </summary>
        public string AgentId { get; }
        
        /// <summary>
        /// Timestamp when the experience was published
        /// </summary>
        public DateTime PublishedAt { get; }

        public ExperiencePublishedEvent(ExperienceId experienceId, string title, DateTime experienceDate, string agentId)
        {
            ExperienceId = experienceId ?? throw new ArgumentNullException(nameof(experienceId));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ExperienceDate = experienceDate;
            AgentId = agentId ?? throw new ArgumentNullException(nameof(agentId));
            PublishedAt = DateTime.UtcNow;
        }
    }
}