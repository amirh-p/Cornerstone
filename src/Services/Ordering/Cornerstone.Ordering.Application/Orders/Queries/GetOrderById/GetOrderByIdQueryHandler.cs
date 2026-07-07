using Cornerstone.Common.Mediator;
using Cornerstone.Common.Exceptions;
using Cornerstone.Ordering.Domain;
using Cornerstone.Ordering.Application.Common.Interfaces;
using Cornerstone.Ordering.Application.Orders.Dtos;

namespace Cornerstone.Ordering.Application.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await orderRepository.GetByIdAsync(OrderId.From(request.Id), ct)
            ?? throw new NotFoundException(nameof(Order), request.Id);

        return new OrderDto(
            order.Id.Value,
            order.CustomerId,
            order.Status.ToString(),
            order.Total.Amount,
            order.Total.Currency,
            order.Lines.Select(l => new Dtos.OrderLineDto(
                l.Product.ProductId,
                l.Product.ProductName,
                l.UnitPrice.Amount,
                l.Quantity,
                l.LineTotal.Amount)).ToList());
    }
}