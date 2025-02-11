namespace Ordering.Domain.Models.ValueObjects;

public record OrderId
{
    public Guid Value { get; }
}