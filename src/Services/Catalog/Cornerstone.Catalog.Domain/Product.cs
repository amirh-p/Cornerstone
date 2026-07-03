using Cornerstone.Common;
using Cornerstone.Catalog.Domain.Events;

namespace Cornerstone.Catalog.Domain;

public sealed class Product : AggregateRoot<ProductId>
{
    public string Name { get; private set; } = default!;
    public string Sku { get; private set; } = default!;
    public Money Price { get; private set; } = default!;
    public bool IsActive { get; private set; }

    // EF Core needs a parameterless constructor — private so it can't be misused elsewhere.
    private Product() { }

    private Product(ProductId id, string name, string sku, Money price) : base(id)
    {
        Name = name;
        Sku = sku;
        Price = price;
        IsActive = true;
    }

    public static Product Create(string name, string sku, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required.", nameof(sku));

        var product = new Product(ProductId.New(), name, sku, price);
        product.Raise(new ProductCreated(product.Id, product.Name, DateTime.UtcNow));
        return product;
    }

    public void ChangePrice(Money newPrice)
    {
        if (newPrice.Currency != Price.Currency)
            throw new InvalidOperationException("Cannot change currency of an existing product.");

        Price = newPrice;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}