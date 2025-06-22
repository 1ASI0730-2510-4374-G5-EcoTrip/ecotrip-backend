using System;
using System.Collections.Generic;

namespace Experience.Application.DTOs
{
    public class ExperienceDetailsDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public int DurationInDays { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public string FormattedPrice { get; set; }

        public string MainImageUrl { get; set; }

        public List<string> ImageUrls { get; set; }

        public string AgentId { get; set; }

        public string AgentName { get; set; }

        public string Status { get; set; }
        public List<ReviewDTO> Reviews { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class ReviewDTO
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
