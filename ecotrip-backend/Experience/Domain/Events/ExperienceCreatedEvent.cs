using System;
using Experience.Domain.ValueObjects;

namespace Experience.Domain.Events
{
    /// <summary>
    /// Event raised when a new experience is created
    /// </summary>
    public class ExperienceCreatedEvent
    {
        /// <summary>
        /// ID of the experience that was created
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
        /// Location of the experience
        /// </summary>
        public string Location { get; }
        
        /// <summary>
        /// Duration of the experience in days
        /// </summary>
        public int DurationInDays { get; }
        
        /// <summary>
        /// ID of the agent who created the experience
        /// </summary>
        public string AgentId { get; }
        
        /// <summary>
        /// Timestamp when the experience was created
        /// </summary>
        public DateTime CreatedAt { get; }

        public ExperienceCreatedEvent(
            ExperienceId experienceId, 
            string title, 
            DateTime experienceDate, 
            string location, 
            int durationInDays, 
            string agentId)
        {
            ExperienceId = experienceId ?? throw new ArgumentNullException(nameof(experienceId));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ExperienceDate = experienceDate;
            Location = location ?? throw new ArgumentNullException(nameof(location));
            DurationInDays = durationInDays > 0 ? durationInDays : throw new ArgumentException("Duration must be greater than 0", nameof(durationInDays));
            AgentId = agentId ?? throw new ArgumentNullException(nameof(agentId));
            CreatedAt = DateTime.UtcNow;
        }
    }
}