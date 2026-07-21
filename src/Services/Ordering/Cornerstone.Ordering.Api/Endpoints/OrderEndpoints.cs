using Cornerstone.Common.Mediator;
using Cornerstone.Ordering.Application.Orders.Commands.PlaceOrder;
using Cornerstone.Ordering.Application.Orders.Dtos;
using Cornerstone.Ordering.Application.Orders.Queries.GetOrderById;

namespace Cornerstone.Ordering.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        group.MapPost("/", async (PlaceOrderRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new PlaceOrderCommand(request.CustomerId, request.Lines);
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/orders/{result.Id}", result);
        })
        .WithName("PlaceOrder")
        .Produces<OrderDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetOrderByIdQuery(id), ct);
            return Results.Ok(result);
        })
        .WithName("GetOrderById")
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }

    public sealed record PlaceOrderRequest(Guid CustomerId, IReadOnlyCollection<PlaceOrderLine> Lines);
}
