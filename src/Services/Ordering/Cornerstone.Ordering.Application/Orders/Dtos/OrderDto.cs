namespace Cornerstone.Ordering.Application.Orders.Dtos;

public sealed record OrderLineDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal Total,
    string Currency,
    IReadOnlyCollection<OrderLineDto> Lines);