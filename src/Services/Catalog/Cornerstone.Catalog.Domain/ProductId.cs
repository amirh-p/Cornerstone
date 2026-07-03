using Cornerstone.Common;

namespace Cornerstone.Catalog.Domain;

public sealed class ProductId : ValueObject
{
    public Guid Value { get; }

    private ProductId(Guid value) => Value = value;

    public static ProductId New() => new(Guid.NewGuid());
    public static ProductId From(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
