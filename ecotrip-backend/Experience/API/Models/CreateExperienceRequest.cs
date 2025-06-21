using System;
using System.ComponentModel.DataAnnotations;

namespace Experience.API.Models
{
    /// <summary>
    /// Request model for creating a new experience
    /// </summary>
    public class CreateExperienceRequest
    {
        /// <summary>
        /// Title of the experience
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters")]
        public string Title { get; set; }

        /// <summary>
        /// Description of the experience
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 2000 characters")]
        public string Description { get; set; }

        /// <summary>
        /// The date when the experience will take place
        /// </summary>
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Location where the experience will take place
        /// </summary>
        [Required(ErrorMessage = "Location is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 100 characters")]
        public string Location { get; set; }

        /// <summary>
        /// Duration of the experience in days
        /// </summary>
        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 30, ErrorMessage = "Duration must be between 1 and 30 days")]
        public int DurationInDays { get; set; }

        /// <summary>
        /// Price of the experience
        /// </summary>
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
        public decimal Price { get; set; }

        /// <summary>
        /// Currency of the price (USD, EUR, etc.)
        /// </summary>
        [Required(ErrorMessage = "Currency is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3-letter code (e.g., USD, EUR)")]
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Main image URL for the experience
        /// </summary>
        [Url(ErrorMessage = "Please provide a valid URL for the main image")]
        public string MainImageUrl { get; set; }
    }