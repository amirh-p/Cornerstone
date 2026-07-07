using Cornerstone.Common;

namespace Cornerstone.Order.Domain;

public sealed class ProductReference : ValueObject
{
    public Guid ProductId { get; }
    public string ProductName { get; }

    private ProductReference(Guid productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }

    public static ProductReference Create(Guid productId, string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.", nameof(productName));

        return new ProductReference(productId, productName);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
        yield return ProductName;
    }
}
