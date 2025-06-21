using System;

namespace Experience.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for basic experience information in listings
    /// </summary>
    public class ExperienceDTO
    {
        /// <summary>
        /// Unique identifier of the experience
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Title of the experience
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Brief summary or excerpt of the experience description
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// The date when the experience will take place
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Location where the experience will take place
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Duration of the experience in days
        /// </summary>
        public int DurationInDays { get; set; }

        /// <summary>
        /// Price amount of the experience
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency code of the price (USD, EUR, etc.)
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Formatted price with currency symbol
        /// </summary>
        public string FormattedPrice { get; set; }

        /// <summary>
        /// URL of the main image for the experience
        /// </summary>
        public string MainImageUrl { get; set; }

        /// <summary>
        /// ID of the agent who created the experience
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// Name of the agent who created the experience
        /// </summary>
        public string AgentName { get; set; }

        /// <summary>
        /// Current status of the experience (Draft, Published, etc.)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Average rating based on reviews
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Total number of reviews
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// When the experience was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}