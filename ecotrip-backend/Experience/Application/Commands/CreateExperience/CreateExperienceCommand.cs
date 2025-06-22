using System;

namespace Experience.Application.Commands.CreateExperience
{    public class CreateExperienceCommand
    {
        public required string Title { get; set; }

        public required string Description { get; set; }
        public DateTime Date { get; set; }
        public required string Location { get; set; }
        public int DurationInDays { get; set; }
        public decimal Price { get; set; }
        public required string Currency { get; set; }
        public required string MainImageUrl { get; set; }
        public string? AgentId { get; set; }
    }
}