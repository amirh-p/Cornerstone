using Cornerstone.Common;

namespace Cornerstone.Ordering.Domain;

public sealed class OrderId : ValueObject
{
    public Guid Value { get; }

    private OrderId(Guid value) => Value = value;

    public static OrderId New() => new(Guid.NewGuid());
    public static OrderId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
