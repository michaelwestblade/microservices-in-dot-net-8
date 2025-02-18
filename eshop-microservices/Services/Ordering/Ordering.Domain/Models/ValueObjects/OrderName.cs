using Ordering.Domain.Exceptions;

namespace Ordering.Domain.Models.ValueObjects;

public record OrderName
{
    private const int DEFAULT_LENGTH = 5;
    public string Value { get; }
    private OrderName(string value) => Value = value;
    
    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DEFAULT_LENGTH);
        
        return new OrderName(value);
    }
}