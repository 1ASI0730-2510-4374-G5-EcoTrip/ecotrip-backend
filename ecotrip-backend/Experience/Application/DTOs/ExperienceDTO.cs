using System;

namespace Experience.Application.DTOs
{
    public class ExperienceDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int DurationInDays { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string FormattedPrice { get; set; }
        public string MainImageUrl { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public string Status { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
