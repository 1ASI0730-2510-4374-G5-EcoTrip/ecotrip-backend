using System;
using Experience.Domain.ValueObjects;

namespace Experience.Domain.Entities
{
    public class Review
    {
        public ReviewId Id { get; private set; }
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsVerified { get; private set; }

        // For ORM
        private Review() { }

        public Review(
            ReviewId id,
            string userId,
            string userName,
            int rating,
            string comment)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            UserId = userId;
            
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("User name cannot be empty", nameof(userName));
            UserName = userName;
            
            SetRating(rating);
            SetComment(comment);
            
            CreatedAt = DateTime.UtcNow;
            IsVerified = false;
        }

        public void SetRating(int rating)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));
            
            Rating = rating;
        }

        public void SetComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Comment cannot be empty", nameof(comment));
            
            if (comment.Length > 1000)
                throw new ArgumentException("Comment cannot exceed 1000 characters", nameof(comment));
            
            Comment = comment;
        }

        public void MarkAsVerified()
        {
            IsVerified = true;
        }

        public override bool Equals(object obj)
        {
            if (obj is Review other)
                return Id.Equals(other.Id);
            
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}