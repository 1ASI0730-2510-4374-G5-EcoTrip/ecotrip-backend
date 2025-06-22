namespace Experience.Domain.ValueObjects;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        }
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be null or empty", nameof(currency));
        }

        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }
        if (Currency != other.Currency)
        {
            throw new InvalidOperationException("Cannot add money with different currencies");
        }

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }
        if (Currency != other.Currency)
        {
            throw new InvalidOperationException("Cannot subtract money with different currencies");
        }

        var result = Amount - other.Amount;
        if (result < 0)
        {
            throw new InvalidOperationException("Result cannot be negative");
        }

        return new Money(result, Currency);
    }

    public Money Multiply(decimal factor)
    {
        if (factor < 0)
        {
            throw new ArgumentException("Factor cannot be negative", nameof(factor));
        }

        return new Money(Amount * factor, Currency);
    }

    public static Money operator +(Money left, Money right)
    {
        return left.Add(right);
    }

    public static Money operator -(Money left, Money right)
    {
        return left.Subtract(right);
    }

    public static Money operator *(Money money, decimal factor)
    {
        return money.Multiply(factor);
    }

    public static Money operator *(decimal factor, Money money)
    {
        return money.Multiply(factor);
    }

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Money);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public static bool operator ==(Money? left, Money? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Money? left, Money? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{Amount:F2} {Currency}";
    }
}
