using Cornerstone.Common.Mediator;
using Cornerstone.Ordering.Application.Orders.Dtos;

namespace Cornerstone.Ordering.Application.Orders.Commands.PlaceOrder;

public sealed record PlaceOrderLine(Guid ProductId, int Quantity);

public sealed record PlaceOrderCommand(Guid CustomerId, IReadOnlyCollection<PlaceOrderLine> Lines) : IRequest<OrderDto>;