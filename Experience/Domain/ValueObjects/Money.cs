using System;
using System.Collections.Generic;
using System.Globalization;

namespace Experience.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a monetary value with currency
    /// </summary>
    public class Money : IEquatable<Money>
    {
        /// <summary>
        /// The amount of money
        /// </summary>
        public decimal Amount { get; }
        
        /// <summary>
        /// The currency code (e.g., USD, EUR, GBP)
        /// </summary>
        public string Currency { get; }
        
        // List of supported currencies
        private static readonly HashSet<string> SupportedCurrencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "USD", "EUR", "GBP", "CAD", "AUD", "JPY", "CNY", "MXN", "BRL", "PEN"
        };

        /// <summary>
        /// Creates a new Money object
        /// </summary>
        /// <param name="amount">The amount</param>
        /// <param name="currency">The currency code</param>
        /// <exception cref="ArgumentException">Thrown when the amount is negative or currency is invalid</exception>
        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));
                
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty", nameof(currency));
                
            if (!SupportedCurrencies.Contains(currency))
                throw new ArgumentException($"Currency '{currency}' is not supported", nameof(currency));
                
            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        /// <summary>
        /// Adds two Money objects with the same currency
        /// </summary>
        /// <param name="other">The money to add</param>
        /// <returns>A new Money object representing the sum</returns>
        /// <exception cref="InvalidOperationException">Thrown when currencies don't match</exception>
        public Money Add(Money other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
                
            if (!Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Cannot add amounts with different currencies: {Currency} and {other.Currency}");
                
            return new Money(Amount + other.Amount, Currency);
        }

        /// <summary>
        /// Subtracts another Money object from this one
        /// </summary>
        /// <param name="other">The money to subtract</param>
        /// <returns>A new Money object representing the difference</returns>
        /// <exception cref="InvalidOperationException">Thrown when currencies don't match</exception>
        public Money Subtract(Money other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
                
            if (!Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Cannot subtract amounts with different currencies: {Currency} and {other.Currency}");
                
            decimal result = Amount - other.Amount;
            if (result < 0)
                throw new InvalidOperationException("Result of subtraction would be negative");
                
            return new Money(result, Currency);
        }

        /// <summary>
        /// Multiplies the money amount by a factor
        /// </summary>
        /// <param name="multiplier">The multiplication factor</param>
        /// <returns>A new Money object with the multiplied amount</returns>
        /// <exception cref="ArgumentException">Thrown when the multiplier is negative</exception>
        public Money Multiply(decimal multiplier)
        {
            if (multiplier < 0)
                throw new ArgumentException("Multiplier cannot be negative", nameof(multiplier));
                
            return new Money(Amount * multiplier, Currency);
        }

        /// <summary>
        /// Returns a formatted string representation of the money value
        /// </summary>
        /// <returns>A formatted string with currency symbol</returns>
        public override string ToString()
        {
            CultureInfo culture;
            
            switch (Currency.ToUpperInvariant())
            {
                case "USD":
                    culture = new CultureInfo("en-US");
                    break;
                case "EUR":
                    culture = new CultureInfo("fr-FR");
                    break;
                case "GBP":
                    culture = new CultureInfo("en-GB");
                    break;
                case "CAD":
                    culture = new CultureInfo("en-CA");
                    break;
                case "AUD":
                    culture = new CultureInfo("en-AU");
                    break;
                case "JPY":
                    culture = new CultureInfo("ja-JP");
                    break;
                case "CNY":
                    culture = new CultureInfo("zh-CN");
                    break;
                case "MXN":
                    culture = new CultureInfo("es-MX");
                    break;
                case "BRL":
                    culture = new CultureInfo("pt-BR");
                    break;
                case "PEN":
                    culture = new CultureInfo("es-PE");
                    break;
                default:
                    culture = CultureInfo.InvariantCulture;
                    return $"{Amount:F2} {Currency}";
            }

            return string.Format(culture, "{0:C}", Amount);
        }

        /// <summary>
        /// Returns a string representation with the amount and currency code
        /// </summary>
        /// <returns>A string with amount and currency code</returns>
        public string ToStringWithCode() => $"{Amount:F2} {Currency}";

        /// <summary>
        /// Determines whether this Money equals another object
        /// </summary>
        /// <param name="obj">The object to compare with</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj) => Equals(obj as Money);

        /// <summary>
        /// Determines whether this Money equals another Money object
        /// </summary>
        /// <param name="other">The Money to compare with</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public bool Equals(Money other)
        {
            if (other == null)
                return false;
                
            return Amount == other.Amount &&
                   string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the hash code for this Money object
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Amount.GetHashCode() * 397) ^ (Currency?.ToUpperInvariant().GetHashCode() ?? 0);
            }
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(Money left, Money right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
                
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(Money left, Money right) => !(left == right);

        /// <summary>
        /// Addition operator
        /// </summary>
        public static Money operator +(Money left, Money right) => left?.Add(right);

        /// <summary>
        /// Subtraction operator
        /// </summary>
        public static Money operator -(Money left, Money right) => left?.Subtract(right);

        /// <summary>
        /// Multiplication operator
        /// </summary>
        public static Money operator *(Money money, decimal multiplier) => money?.Multiply(multiplier);

        /// <summary>
        /// Multiplication operator (reversed operands)
        /// </summary>
        public static Money operator *(decimal multiplier, Money money) => money?.Multiply(multiplier);
    }
}