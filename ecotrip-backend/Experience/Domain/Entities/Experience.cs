using System;
using System.Collections.Generic;
using Experience.Domain.Enums;
using Experience.Domain.ValueObjects;

namespace Experience.Domain.Entities
{
    public class Experience
    {
        public ExperienceId Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }
        public string Location { get; private set; }
        public int DurationInDays { get; private set; }
        public Money Price { get; private set; }
        public List<string> ImageUrls { get; private set; } = new List<string>();
        public string MainImageUrl { get; private set; }
        public string AgentId { get; private set; }
        public ExperienceStatus Status { get; private set; }
        public List<Review> Reviews { get; private set; } = new List<Review>();
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // For ORM
        private Experience() { }

        public Experience(
            ExperienceId id,
            string title,
            string description,
            DateTime date,
            string location,
            int durationInDays,
            Money price,
            string mainImageUrl,
            string agentId)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            UpdateTitle(title);
            UpdateDescription(description);
            Date = date;
            Location = location ?? throw new ArgumentNullException(nameof(location));
            DurationInDays = durationInDays > 0 ? durationInDays : throw new ArgumentException("Duration must be greater than 0", nameof(durationInDays));
            Price = price ?? throw new ArgumentNullException(nameof(price));
            MainImageUrl = mainImageUrl ?? throw new ArgumentNullException(nameof(mainImageUrl));
            AgentId = agentId ?? throw new ArgumentNullException(nameof(agentId));
            Status = ExperienceStatus.Draft;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));
                
            Title = title;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty", nameof(description));
                
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDate(DateTime date)
        {
            if (date < DateTime.UtcNow.Date)
                throw new ArgumentException("Experience date cannot be in the past", nameof(date));
                
            Date = date;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be empty", nameof(location));
                
            Location = location;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDuration(int durationInDays)
        {
            if (durationInDays <= 0)
                throw new ArgumentException("Duration must be greater than 0", nameof(durationInDays));
                
            DurationInDays = durationInDays;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePrice(Money price)
        {
            Price = price ?? throw new ArgumentNullException(nameof(price));
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be empty", nameof(imageUrl));
                
            ImageUrls.Add(imageUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetMainImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be empty", nameof(imageUrl));
                
            MainImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveImage(string imageUrl)
        {
            ImageUrls.Remove(imageUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void Publish()
        {
            if (Status == ExperienceStatus.Published)
                throw new InvalidOperationException("Experience is already published");
                
            Status = ExperienceStatus.Published;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            if (Status != ExperienceStatus.Published)
                throw new InvalidOperationException("Experience is not published");
                
            Status = ExperienceStatus.Draft;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == ExperienceStatus.Cancelled)
                throw new InvalidOperationException("Experience is already cancelled");
                
            Status = ExperienceStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status != ExperienceStatus.Published)
                throw new InvalidOperationException("Only published experiences can be marked as completed");
            
            if (DateTime.UtcNow < Date)
                throw new InvalidOperationException("Cannot complete an experience before its date");
                
            Status = ExperienceStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddReview(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review));
                
            if (Status != ExperienceStatus.Completed)
                throw new InvalidOperationException("Reviews can only be added to completed experiences");
                
            Reviews.Add(review);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}