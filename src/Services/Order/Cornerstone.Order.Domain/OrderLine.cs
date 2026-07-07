using Cornerstone.Common;

namespace Cornerstone.Order.Domain;

public sealed class OrderLine
{
    public ProductReference Product { get; }
    public Money UnitPrice { get; }
    public int Quantity { get; private set; }

    public Money LineTotal => Money.Create(UnitPrice.Amount * Quantity, UnitPrice.Currency);

    internal OrderLine(ProductReference product, Money unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        Product = product;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void IncreaseQuantity(int amount) => Quantity += amount;
}
