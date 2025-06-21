using System;
using System.Collections.Generic;

namespace Experience.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object containing detailed information about an experience
    /// </summary>
    public class ExperienceDetailsDTO
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
        /// Detailed description of the experience
        /// </summary>
        public string Description { get; set; }

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
        /// Collection of additional image URLs for the experience
        /// </summary>
        public List<string> ImageUrls { get; set; }

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
        /// Collection of reviews for the experience
        /// </summary>
        public List<ReviewDTO> Reviews { get; set; }

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

        /// <summary>
        /// When the experience was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Data Transfer Object for review information
    /// </summary>
    public class ReviewDTO
    {
        /// <summary>
        /// Unique identifier of the review
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ID of the user who wrote the review
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Name of the user who wrote the review
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Rating given in the review (1-5)
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Comment text in the review
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Whether the review is from a verified purchaser
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// When the review was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}