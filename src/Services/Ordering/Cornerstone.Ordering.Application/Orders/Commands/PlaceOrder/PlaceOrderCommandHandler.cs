using Cornerstone.Common.Mediator;
using Cornerstone.Common.Exceptions;
using Cornerstone.Ordering.Domain;
using Cornerstone.Ordering.Application.Common.Interfaces;
using Cornerstone.Ordering.Application.Orders.Dtos;

namespace Cornerstone.Ordering.Application.Orders.Commands.PlaceOrder;

public sealed class PlaceOrderCommandHandler(IOrderRepository orderRepository, IProductCatalogClient catalogClient)
    : IRequestHandler<PlaceOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        var order = Order.Create(request.CustomerId);

        foreach (var line in request.Lines)
        {
            var product = await catalogClient.GetProductAsync(line.ProductId, ct)
                ?? throw new NotFoundException(nameof(ProductSnapshot), line.ProductId);

            order.AddLine(product.ProductId, product.Name, product.Price, product.Currency, line.Quantity);
        }

        order.Place();

        await orderRepository.AddAsync(order, ct);
        await orderRepository.SaveChangesAsync(ct);

        return ToDto(order);
    }

    private static OrderDto ToDto(Order order) => new(
        order.Id.Value,
        order.CustomerId,
        order.Status.ToString(),
        order.Total.Amount,
        order.Total.Currency,
        order.Lines.Select(l => new OrderLineDto(
            l.Product.ProductId, l.Product.ProductName, l.UnitPrice.Amount, l.Quantity, l.LineTotal.Amount)).ToList());
}