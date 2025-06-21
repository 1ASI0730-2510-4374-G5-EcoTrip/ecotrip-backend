using System;

namespace Experience.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a unique identifier for an Experience
    /// </summary>
    public class ExperienceId
    {
        /// <summary>
        /// The underlying ID value
        /// </summary>
        public Guid Value { get; }

        /// <summary>
        /// Creates a new ExperienceId with the specified GUID
        /// </summary>
        /// <param name="id">The GUID value</param>
        /// <exception cref="ArgumentException">Thrown when the provided ID is empty</exception>
        public ExperienceId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Experience ID cannot be empty", nameof(id));
                
            Value = id;
        }

        /// <summary>
        /// Factory method to create a new ExperienceId with a random GUID
        /// </summary>
        /// <returns>A new ExperienceId instance</returns>
        public static ExperienceId Create() => new ExperienceId(Guid.NewGuid());

        /// <summary>
        /// Factory method to parse a string representation of a GUID into an ExperienceId
        /// </summary>
        /// <param name="id">String representation of a GUID</param>
        /// <returns>An ExperienceId instance</returns>
        /// <exception cref="ArgumentException">Thrown when the provided string is not a valid GUID</exception>
        public static ExperienceId Parse(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                throw new ArgumentException("Invalid Experience ID format", nameof(id));
                
            return new ExperienceId(guid);
        }

        /// <summary>
        /// Returns the string representation of the ExperienceId
        /// </summary>
        /// <returns>The string representation of the ID</returns>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// Determines whether this ExperienceId equals another object
        /// </summary>
        /// <param name="obj">The object to compare with</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is ExperienceId other)
                return Value.Equals(other.Value);
            return false;
        }

        /// <summary>
        /// Returns the hash code for this ExperienceId
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(ExperienceId left, ExperienceId right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(ExperienceId left, ExperienceId right) => !(left == right);
    }
}