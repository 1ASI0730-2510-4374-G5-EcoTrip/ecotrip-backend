using System;

namespace Experience.Application.Commands.CreateExperience
{
    /// <summary>
    /// Command to create a new experience
    /// </summary>
    public class CreateExperienceCommand
    {
        /// <summary>
        /// Title of the experience
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Description of the experience
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The date when the experience will take place
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Location where the experience will take place
        /// </summary>
        public required string Location { get; set; }

        /// <summary>
        /// Duration of the experience in days
        /// </summary>
        public int DurationInDays { get; set; }

        /// <summary>
        /// Price of the experience
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency of the price (USD, EUR, etc.)
        /// </summary>
        public required string Currency { get; set; }

        /// <summary>
        /// Main image URL for the experience
        /// </summary>
        public required string MainImageUrl { get; set; }

        /// <summary>
        /// ID of the agent creating the experience
        /// </summary>
        public string? AgentId { get; set; }
    }
}