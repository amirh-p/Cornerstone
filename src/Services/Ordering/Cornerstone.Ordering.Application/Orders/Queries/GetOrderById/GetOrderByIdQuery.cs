using Cornerstone.Common.Mediator;
using Cornerstone.Ordering.Application.Orders.Dtos;

namespace Cornerstone.Ordering.Application.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;