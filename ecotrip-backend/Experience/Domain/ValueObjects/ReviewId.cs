namespace Experience.Domain.ValueObjects;

public class ReviewId : IEquatable<ReviewId>
{
    public Guid Value { get; }

    public ReviewId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("ReviewId cannot be empty", nameof(value));
        }
        Value = value;
    }

    public static ReviewId Create()
    {
        return new ReviewId(Guid.NewGuid());
    }

    public static ReviewId Create(Guid value)
    {
        return new ReviewId(value);
    }

    public bool Equals(ReviewId? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ReviewId);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(ReviewId? left, ReviewId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ReviewId? left, ReviewId? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Guid(ReviewId reviewId)
    {
        return reviewId.Value;
    }

    public static implicit operator ReviewId(Guid value)
    {
        return new ReviewId(value);
    }
}
