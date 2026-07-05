using Cornerstone.Catalog.Application.Products.Commands.CreateProduct;
using Cornerstone.Catalog.Application.Products.Dtos;
using Cornerstone.Catalog.Application.Products.Queries.GetProductById;
using Cornerstone.Common.Mediator;

namespace Cornerstone.Catalog.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapPost("/", async (CreateProductRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new CreateProductCommand(request.Name, request.Sku, request.Price, request.Currency);
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/products/{result.Id}", result);
        })
        .WithName("CreateProduct")
        .Produces<ProductDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id), ct);
            return Results.Ok(result);
        })
        .WithName("GetProductById")
        .Produces<ProductDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}

public sealed record CreateProductRequest(string Name, string Sku, decimal Price, string Currency);