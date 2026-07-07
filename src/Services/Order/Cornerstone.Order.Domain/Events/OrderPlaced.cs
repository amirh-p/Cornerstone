using Cornerstone.Common;

namespace Cornerstone.Order.Domain.Events;

public sealed record OrderPlacedLine(Guid ProductId, int Quantity);

public sealed record OrderPlaced(
    OrderId OrderId,
    Guid CustomerId,
    IReadOnlyCollection<OrderPlacedLine> Lines,
    DateTime OccurredOn) : IDomainEvent;
