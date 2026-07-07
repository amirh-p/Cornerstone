using Cornerstone.Common;
using Cornerstone.Order.Domain.Events;

namespace Cornerstone.Order.Domain;

public sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderLine> _lines = [];
    public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Money Total => _lines.Count == 0
        ? Money.Create(0, "EUR")
        : Money.Create(_lines.Sum(l => l.LineTotal.Amount), _lines.First().LineTotal.Currency);

    private Order() { }

    private Order(OrderId id, Guid customerId) : base(id)
    {
        CustomerId = customerId;
        Status = OrderStatus.Draft;
        CreatedAt = DateTime.UtcNow;
    }

    public static Order Create(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId is required.", nameof(customerId));

        return new Order(OrderId.New(), customerId);
    }

    public void AddLine(Guid productId, string productName, decimal unitPrice, string currency, int quantity)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Cannot modify an order that has already been placed.");

        var existingLine = _lines.FirstOrDefault(l => l.Product.ProductId == productId);
        if (existingLine is not null)
        {
            existingLine.IncreaseQuantity(quantity);
            return;
        }

        var product = ProductReference.Create(productId, productName);
        var price = Money.Create(unitPrice, currency);
        _lines.Add(new OrderLine(product, price, quantity));
    }

    public void Place()
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Order has already been placed.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot place an order with no lines.");

        Status = OrderStatus.Placed;

        var lines = _lines.Select(l => new OrderPlacedLine(l.Product.ProductId, l.Quantity)).ToList();
        Raise(new OrderPlaced(Id, CustomerId, lines, DateTime.UtcNow));
    }

    public void Cancel(string reason)
    {
        if (Status is OrderStatus.Paid or OrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel an order in '{Status}' status.");

        Status = OrderStatus.Cancelled;
        Raise(new OrderCancelled(Id, reason, DateTime.UtcNow));
    }
}
